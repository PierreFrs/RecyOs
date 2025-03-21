using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog;
using Polly;
using Polly.Timeout;
using Polly.Wrap;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using RecyOs.ORM.Interfaces.ICron;
using RecyOs.ORM.Models.DTO.hub;

namespace RecyOs.Cron;

public class SyncBalanceCron : ISyncBalanceCron
{
    private readonly IMoveAccountService _moveAccountService;
    private readonly IEtablissementClientService _etablissementClientService;
    private readonly IClientEuropeService _clientEuropeService;
    private readonly IClientParticulierService _clientParticulierService;
    private readonly ISocieteBaseService _societeService;
    private readonly IBalanceFranceService _balanceFranceService;
    private readonly IAcountAccountService _accountAccountService;
    private readonly IBalanceEuropeService _balanceEuropeService;
    private readonly IBalanceParticulierService _balanceParticulierService;
    private readonly ITokenInfoService _tokenInfoService;
    private IReadOnlyList<SocieteDto> _societesCache;
    private IList<long> _accounts;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public SyncBalanceCron(
        IMoveAccountService moveAccountService,
        IEtablissementClientService etablissementClientService,
        IClientEuropeService clientEuropeService,
        IClientParticulierService clientParticulierService,
        ISocieteBaseService societeService,
        IBalanceFranceService balanceFranceService,
        IAcountAccountService accountAccountService,
        IBalanceEuropeService balanceEuropeService,
        IBalanceParticulierService balanceParticulierService,
        ITokenInfoService tokenInfoService)
    {
        _moveAccountService = moveAccountService;
        _etablissementClientService = etablissementClientService;
        _clientEuropeService = clientEuropeService;
        _clientParticulierService = clientParticulierService;
        _societeService = societeService;
        _balanceFranceService = balanceFranceService;
        _balanceEuropeService = balanceEuropeService;
        _balanceParticulierService = balanceParticulierService;
        _accountAccountService = accountAccountService;
        _tokenInfoService = tokenInfoService;
    }
    
    public async Task ExecuteAsync()
    {
        _societesCache = await _societeService.GetListAsync();

        // Combine retry et timeout
        var policy = await GetResiliencePolicy();

        _logger.Trace("Détermine la liste des comptes à extraire");
        _accounts = new List<long>();
        _accounts = _societesCache.Select(GetAccountsForSociete)
            .SelectMany(accounts => accounts) // Flatten the results
            .ToList();

        await ImportBalancesForAllClients(policy);
    }

    // The method is marked as async but doesn't use any await operations internally
    private Task<AsyncPolicyWrap> GetResiliencePolicy()
    {
        var timeoutPolicy = Policy.TimeoutAsync(300, TimeoutStrategy.Pessimistic); // Timeout configurable
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<InvalidOperationException>()
            .WaitAndRetryAsync(
                3,
                retryAttempt => TimeSpan.FromSeconds(retryAttempt),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.Warn($"Tentative {retryCount} échouée après {timeSpan.TotalSeconds}s. Erreur : {exception.Message}");
                });

        var policyWrap = Policy.WrapAsync(retryPolicy, timeoutPolicy);
        return Task.FromResult(policyWrap);
    }

    private IList<long> GetAccountsForSociete(SocieteDto societe)
    {
        return _accountAccountService.GetCustomerAccountList(long.Parse(societe.IdOdoo))
            .Select(acc => acc.Id)
            .ToList();
    }

    private async Task ImportBalancesForAllClients(AsyncPolicyWrap policy)
    {
        // Calls the method to import balances for each type of clients
        await ImportClientsBalances<EtablissementClientDto>(() => _etablissementClientService.GetAll(false), "France", policy);
        await ImportClientsBalances<ClientEuropeDto>(() => _clientEuropeService.GetAll(false), "Europe", policy);
        await ImportClientsBalances<ClientParticulierDto>(() => _clientParticulierService.GetAll(false), "Particulier", policy);
    }


    private async Task ImportClientsBalances<T>(
        Func<Task<IList<T>>> getClients,
        string region,
        AsyncPolicyWrap policy)
        where T : IClientBalanceDto
    {
        _logger.Trace($"Importe les balances clients pour {region}");

        // Fetch all clients from the appropriate service
        var clients = await getClients();

        foreach (var client in clients.Where(x => !string.IsNullOrEmpty(x.IdOdoo)))
        {
            await ImportClientBalance(client, policy);
        }
    }

    private async Task ImportClientBalance(IClientBalanceDto client, AsyncPolicyWrap policy)
    {
        if (!string.IsNullOrEmpty(client.IdOdoo))
        {
            try
            {
                await policy.ExecuteAsync(async () =>
                {
                    await UpdateSocieteBalance(client);
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Une erreur inattendue est survenue.", ex);
            }
        }
    }

    
    private async Task UpdateSocieteBalance(IClientBalanceDto client)
    {
        _logger.Trace("MAJ Balances du client : " + client.Nom);

        try
        {
            // Récupère les lignes comptables du client
            var lines = await GetClientAccountLines(client);

            if (lines == null || !lines.Any())
            {
                _logger.Warn($"Pas de lignes comptables pour le client {client.Nom}");
                return;
            }

            foreach (var societe in _societesCache)
            {
                if (client is EtablissementClientDto etablissement)
                    await ProcessSocieteBalance<EtablissementClientDto>(etablissement, societe, lines);
                else if (client is ClientEuropeDto clientEurope)
                    await ProcessSocieteBalance<ClientEuropeDto>(clientEurope, societe, lines);
                else if (client is ClientParticulierDto clientParticulier)
                    await ProcessSocieteBalance<ClientParticulierDto>(clientParticulier, societe, lines);
                else
                    throw new InvalidOperationException($"Unsupported client type: {client.GetType().Name}");

            }
        }
        catch (FormatException ex)
        {
            _logger.Error(ex, "Problème: l'idOdoo du client {ClientName} n'est pas valide.", client.Nom);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erreur inattendue lors de la mise à jour de la balance pour {ClientName}", client.Nom);
        }
    }

    private async Task<IList<AccountMoveLineOdooModel>> GetClientAccountLines(IClientBalanceDto client)
    {
        return await _moveAccountService.GetAccountLines(long.Parse(client.IdOdoo),
            DateTime.Now.ToString("yyyy-MM-dd"), _accounts.ToArray());
    }

    private async Task ProcessSocieteBalance<T>(IClientBalanceDto client, SocieteDto societe, IList<AccountMoveLineOdooModel> lines)
    {
        try
        {
            var accountList = _accountAccountService.GetCustomerAccountList(long.Parse(societe.IdOdoo));
            decimal balance = CalculateTotalBalance(lines, accountList);

            await SaveBalance<T>(client, societe, balance);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erreur lors du traitement de la balance pour {ClientName} et {SocieteName}", client.Nom, societe.Nom);
        }
    }

    private IBalanceService<BalanceDto> GetBalanceService<T>()
    {
        if (typeof(T) == typeof(EtablissementClientDto))
            return _balanceFranceService;
        if (typeof(T) == typeof(ClientEuropeDto))
            return _balanceEuropeService;
        if (typeof(T) == typeof(ClientParticulierDto))
            return _balanceParticulierService;

        throw new InvalidOperationException($"No balance service found for type {typeof(T).Name}");
    }


    private decimal CalculateTotalBalance(IList<AccountMoveLineOdooModel> lines, IReadOnlyList<AccountAccountOdooModel> accountList)
    {
        decimal balance = 0;

        foreach (var account in accountList)
        {
            try
            {
                balance += CalculateBalance(lines, account.Id);
            }
            catch (Exception ex)
            {
                _logger.Warn("Impossible de calculer la balance pour le compte {AccountId}. Erreur: {Error}", account.Id, ex.Message);
            }
        }

        return balance;
    }

    private static Decimal CalculateBalance(IList<AccountMoveLineOdooModel> lines, long accountId)
    {
        var totalDebit = lines.Where(x => x.AccountId == accountId).Sum(x => x.Debit)
                         ?? throw new InvalidOperationException("Le total du débit est indéterminé.");
        var totalCredit = lines.Where(x => x.AccountId == accountId).Sum(x => x.Credit)
                          ?? throw new InvalidOperationException("Le total du crédit est indéterminé.");
        var balance = totalDebit - totalCredit;

        return balance;
    }

    private async Task SaveBalance<T>(IClientBalanceDto client, SocieteDto societe, decimal balance)
    {
        var balanceDto = new BalanceDto
        {
            ClientId = client.Id,
            SocieteId = societe.Id,
            DateRecuperationBalance = DateTime.Now,
            Montant = balance,
            CreateDate = DateTime.Now,
            CreatedBy = _tokenInfoService.GetCurrentUserName(),
        };

        try
        {
            IBalanceService<BalanceDto> balanceService = GetBalanceService<T>();

            await balanceService.CreateAsync(balanceDto);
            _logger.Trace("Balance enregistrée pour {ClientName} et {SocieteName} : {Balance}", client.Nom, societe.Nom, balance);
        }
        catch (DbUpdateException dbEx) when (dbEx.InnerException?.Message.Contains("UNIQUE constraint failed") == true ||
                                             dbEx.InnerException?.Message.Contains("duplicate key value violates unique constraint") == true)
        {
            _logger.Warn("Balance déjà existante pour {ClientName} et {SocieteName}, non enregistrée.", client.Nom, societe.Nom);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Erreur lors de l'enregistrement de la balance pour {ClientName}", client.Nom);
        }
    }
}