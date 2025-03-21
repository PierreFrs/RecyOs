using Microsoft.Extensions.Hosting;
using NLog;
using RecyOs.OdooImporter.Interfaces;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Models.Entities.hub.IncomeOdoo;
using Cronos;

namespace RecyOs.OdooImporter.Services;

public class OdooImporterService : BackgroundService
{
    private readonly IClientRepository _clientRepository;
    private readonly OdooAccountMoveLinesService _accountMoveLinesService;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly IBalanceFranceRepository _balanceFranceRepository;
    private readonly IBalanceEuropeRepository _balanceEuropeRepository;
    private readonly IBalanceParticulierRepository _balanceParticulierRepository;
    private readonly IAccountMoveLineRepository _accountMoveLineRepository;
    private readonly CronExpression _cronExpression;
    
    public OdooImporterService(
        IClientRepository clientRepository,
        OdooAccountMoveLinesService accountMoveLinesService,
        IBalanceFranceRepository balanceRepository,
        IBalanceEuropeRepository balanceEuropeRepository,
        IBalanceParticulierRepository balanceParticulierRepository,
        IAccountMoveLineRepository accountMoveLineRepository)
    {
        _clientRepository = clientRepository;
        _accountMoveLinesService = accountMoveLinesService;
        _balanceFranceRepository = balanceRepository;
        _balanceEuropeRepository = balanceEuropeRepository;
        _balanceParticulierRepository = balanceParticulierRepository;
        _accountMoveLineRepository = accountMoveLineRepository;
        _cronExpression = CronExpression.Parse("0 2 * * *");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var nextRun = _cronExpression.GetNextOccurrence(DateTime.UtcNow, TimeZoneInfo.Local);
            if (nextRun.HasValue)
            {
                var delay = nextRun.Value - DateTime.UtcNow;
                if (delay > TimeSpan.Zero)
                {
                    _logger.Info($"Prochaine exécution planifiée pour le {nextRun.Value.ToLocalTime()}");
                    await Task.Delay(delay, stoppingToken);
                }

                try
                {
                    await _accountMoveLinesService.InitializeAsync();
                    await ImportBalancesFranceAsync();
                    await ImportBalancesEuropeAsync();
                    await ImportBalancesParticuliersAsync();
                    _logger.Info("Importation des balances terminée avec succès");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erreur lors de l'importation des balances");
                }
            }
        }
    }

    private async Task ImportBalancesFranceAsync()
    {
        var clients = await _clientRepository.GetAllFrenchAsync();
        foreach (var client in clients)
        {
            _logger.Trace($"Récupération des lignes comptables pour le client {client.Nom}");
            IList<AccountMoveLine> lines = await _accountMoveLinesService.GetAccountLinesAsync(long.Parse(client.IdOdoo));
            Dictionary<int, decimal> balances = _accountMoveLinesService.ProcessSocietesBalance(lines);
            await _accountMoveLineRepository.AddRangeAsync(lines);

            foreach (var balance in balances)
            {
                _logger.Trace($"Balance du client {client.Nom} pour le compte {balance.Key}: {balance.Value}");
                await _balanceFranceRepository.SaveBalance(client.Id, balance.Key, balance.Value);
            }
        }
    }

    private async Task ImportBalancesEuropeAsync()
    {
        var clients = await _clientRepository.GetAllEuropeanAsync();
        foreach (var client in clients)
        {
            _logger.Trace($"Récupération des lignes comptables pour le client {client.Nom}");
            IList<AccountMoveLine> lines = await _accountMoveLinesService.GetAccountLinesAsync(long.Parse(client.IdOdoo));
            Dictionary<int, decimal> balances = _accountMoveLinesService.ProcessSocietesBalance(lines);
            await _accountMoveLineRepository.AddRangeAsync(lines);

            foreach (var balance in balances)
            {
                _logger.Trace($"Balance du client {client.Nom} pour le compte {balance.Key}: {balance.Value}");
                await _balanceEuropeRepository.SaveBalance(client.Id, balance.Key, balance.Value);
            }
        }
    }

    private async Task ImportBalancesParticuliersAsync()
    {
        var clients = await _clientRepository.GetAllIndividualAsync();
        foreach (var client in clients)
        {
            _logger.Trace($"Récupération des lignes comptables pour le client {client.Nom}");
            IList<AccountMoveLine> lines = await _accountMoveLinesService.GetAccountLinesAsync(long.Parse(client.IdOdoo));
            Dictionary<int, decimal> balances = _accountMoveLinesService.ProcessSocietesBalance(lines);
            await _accountMoveLineRepository.AddRangeAsync(lines);
            
            foreach (var balance in balances)
            {
                _logger.Trace($"Balance du client {client.Nom} pour le compte {balance.Key}: {balance.Value}");
                await _balanceParticulierRepository.SaveBalance(client.Id, balance.Key, balance.Value);
            }
        }
    }
}