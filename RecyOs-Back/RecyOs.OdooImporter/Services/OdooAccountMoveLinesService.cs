using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using RecyOs.OdooDB.Interfaces;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Models.Entities.hub.IncomeOdoo;
using Newtonsoft.Json.Linq;
using RecyOs.OdooImporter.Interfaces;

namespace RecyOs.OdooImporter.Services;

public class OdooAccountMoveLinesService
{
    private readonly IRecyOsOdooClient _odooClient;
    private readonly IAcountAccountService _accountAccountService;
    private readonly ISocieteRepository _societeRepository;
    private readonly IAccountMoveLineRepository _accountMoveLineRepository;
    private IEnumerable<Societe>? _societesCache;
    private IList<long>? _accounts;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public OdooAccountMoveLinesService(
        IRecyOsOdooClient odooClient,
        IAcountAccountService accountAccountService,
        ISocieteRepository societeRepository,
        IAccountMoveLineRepository accountMoveLineRepository)
    {
        _odooClient = odooClient;
        _accountAccountService = accountAccountService;
        _societeRepository = societeRepository;
        _accountMoveLineRepository = accountMoveLineRepository;
    }

    public async Task InitializeAsync()
    {
        await _accountMoveLineRepository.EraseAllAsync();
        _societesCache = await _societeRepository.GetAllAsync();
        _logger.Trace("Détermine la liste des comptes à extraire");
        _accounts = new List<long>();
        _accounts = _societesCache.Select(GetAccountsForSociete)
            .SelectMany(accounts => accounts)
            .ToList();
        _logger.Trace("Liste des comptes à extraire: {0}", _accounts.Count);
    }

    public async Task<IList<AccountMoveLine>> GetAccountLinesAsync(long partnerId)
    {
        if (_accounts == null)
        {
            _logger.Error("La liste des comptes n'a pas été initialisée");
            return new List<AccountMoveLine>();
        }

        var domain = new[]
        {
            new object[] { "partner_id", "=", partnerId },
            new object[] { "date", "<=", DateTime.Now.ToString("yyyy-MM-dd") },
            new object[] { "account_id", "in", _accounts.ToArray() },
            new object[] { "parent_state", "=", "posted" },
            new object[] { "reconciled", "=", false }
        };

        var fields = new[]
        {
            "id", "move_id", "move_name", "date", "partner_id",
            "debit", "credit", "balance", "date_maturity",
            "reconciled", "account_id"
        };

        var accountMoveLines = await _odooClient.SearchRead(
            "account.move.line",
            domain,
            fields
        );

        return accountMoveLines.Select(line => new AccountMoveLine
        {
            Id = GetIdValue(line["id"]),
            MoveId = GetIdValue(line["move_id"]),
            MoveName = line["move_name"]?.ToString(),
            Date = ParseDate(line["date"]),
            PartnerId = partnerId,
            Debit = line["debit"] != null ? Convert.ToDecimal(line["debit"].ToString()) : null,
            Credit = line["credit"] != null ? Convert.ToDecimal(line["credit"].ToString()) : null,
            Balance = line["balance"] != null ? Convert.ToDecimal(line["balance"].ToString()) : null,
            DateMaturity = ParseDate(line["date_maturity"]),
            Reconciled = line["reconciled"] != null ? Convert.ToBoolean(line["reconciled"].ToString()) : null,
            AccountId = long.Parse(line["account_id"].ToString().Split(',')[0].Replace("[", "").Replace("]", ""))
        }).ToList();
    }

    public Dictionary<int, decimal> ProcessSocietesBalance(IList<AccountMoveLine> lines)
    {
        Dictionary<int, decimal> balances = new Dictionary<int, decimal>();

        if (_societesCache == null)
        {
            _logger.Error("La liste des sociétés n'a pas été initialisée");
            return balances;
        }

        foreach (var societe in _societesCache)
        {
            var accountList = _accountAccountService.GetCustomerAccountList(long.Parse(societe.IdOdoo));
            IList<long> accountsIds = accountList.Select(a => a.Id).ToList();
            decimal? balance = CalculateTotalBalance(lines, accountsIds);
            balances[societe.Id] = balance ?? 0;
        }
        return balances;
    }

    private static decimal? CalculateTotalBalance(IList<AccountMoveLine> lines, IList<long> accountList)
    {
        var filteredLines = lines.Where(line => accountList.Contains(line.AccountId));
        return filteredLines.Sum(line => line.Credit - line.Debit);
    }

    private IList<long> GetAccountsForSociete(Societe societe)
    {
        return _accountAccountService.GetCustomerAccountList(long.Parse(societe.IdOdoo))
            .Select(acc => acc.Id)
            .ToList();
    }

    private DateTime? ParseDate(object value)
    {
        if (value == null) return null;
        
        try
        {
            var dateStr = value.ToString();
            if (string.IsNullOrEmpty(dateStr) || dateStr.ToLower() == "false") return null;
            
            return DateTime.Parse(dateStr);
        }
        catch (Exception ex)
        {
            _logger.Warn($"Impossible de parser la date '{value}': {ex.Message}");
            return null;
        }
    }

    private long GetIdValue(object value)
    {
        try
        {
            if (value is JArray jArray)
            {
                return Convert.ToInt64(jArray[1]);
            }
            if (value is JValue jValue)
            {
                return jValue.Value<long>();
            }
            return Convert.ToInt64(value);
        }
        catch (FormatException)
        {
            try
            {
                string test = value.ToString().Split(',')[0].Replace("[", "").Replace("]", "");
                test = test.Replace(" ", "").Replace("'", "");
                if (long.TryParse(test, out long result))
                {
                    _logger.Trace($"La valeur '{value}' a été convertie en ID numérique: {result}");
                    return result;
                }
                else
                {
                    _logger.Warn($"La valeur '{value}' ne peut pas être convertie en ID numérique retourne 0");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Erreur lors de la conversion de l'ID: {value}");
                return 0;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Erreur lors de la conversion de l'ID: {value}");
            return 0;
        }
    }
}
