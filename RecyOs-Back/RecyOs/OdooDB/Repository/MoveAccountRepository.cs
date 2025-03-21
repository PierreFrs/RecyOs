using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PortaCapena.OdooJsonRpcClient;
using PortaCapena.OdooJsonRpcClient.Request;
using PortaCapena.OdooJsonRpcClient.Result;
using RecyOs.Engine.Services;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;

namespace RecyOs.OdooDB.Repository;

public class MoveAccountRepository: BaseOdooRepository, IMoveAccountRepository
{
    private readonly OdooRepository<AccountMoveLineOdooModel> _odooRepository;
    
    public MoveAccountRepository(IConfiguration configuration) : base(configuration)
    {
        _odooRepository = new OdooRepository<AccountMoveLineOdooModel>(_odooConfig);
    }

    public async Task<IList<AccountMoveLineOdooModel>> GetAccountMoveLines(string prmDate, long accountId)
    {
        var filter = OdooFilter<AccountMoveLineOdooModel>.Create()
            .EqualTo( x => x.AccountId, accountId )
            .EqualTo( x => x.ParentState, "posted" )
            .EqualTo( x => x.Reconciled, false )
            .LessThanOrEqual(x => x.Date, prmDate); // Assurez-vous que Date est correctement mappé
        
        var accountMoveLines = await _odooRepository.Query()
            .Where(filter)
            .ToListAsync();

        return accountMoveLines.Value;
    }
    
    
    /// <summary>
    /// Returns the account balance for a given partner, business, and date.
    /// </summary>
    /// <param name="partnerId">The ID of the partner.</param>
    /// <param name="companyId">The ID of the business.</param>
    /// <param name="prmDate">The date to calculate the balance.</param>
    /// <returns>The calculated account balance.</returns>
    public async Task<decimal> GetAccountBalance(long partnerId, long companyId, string prmDate, long accountId)
    {
        var filter = OdooFilter<AccountMoveLineOdooModel>.Create()
            .EqualTo(x => x.PartnerId, accountId) // Assurez-vous que PartnerId est correctement mappé dans votre modèle
            .EqualTo( x => x.AccountId, accountId )
            .EqualTo( x => x.ParentState, "posted" )
            .EqualTo( x => x.Reconciled, false )
            .LessThanOrEqual(x => x.Date, prmDate); // Assurez-vous que Date est correctement mappé

        var accountMoveLines = await _odooRepository.Query()
            .Where(filter)
            .ToListAsync();
        
        // Assurez-vous que accountMoveLines.Value n'est pas null ni vide.
        if (accountMoveLines.Value == null || !accountMoveLines.Value.Any())
        {
            throw new InvalidOperationException("Aucune ligne comptable trouvée.");
        }

        // Calculez la balance en utilisant les lignes récupérées
        var totalDebit = accountMoveLines.Value.Sum(x => x.Debit)
                        ?? throw new InvalidOperationException("Le total du débit est indéterminé.");
        var totalCredit = accountMoveLines.Value.Sum(x => x.Credit)
                        ?? throw new InvalidOperationException("Le total du crédit est indéterminé.");
        var balance = totalDebit - totalCredit;

        Console.WriteLine("Nombre de lignes remontées : " + accountMoveLines.Value.Count());
        return balance;
    }
    
    public async Task<IList<AccountMoveLineOdooModel>> GetAccountLines(long partnerId, string prmDate, long[] accountIds)
{
    try
    {
        // Créez le filtre pour inclure plusieurs accountIds
        var filter = OdooFilter<AccountMoveLineOdooModel>.Create()
            .EqualTo(x => x.PartnerId, partnerId) // Vérifiez que PartnerId est correctement mappé dans votre modèle
            .In(x => x.AccountId, accountIds) // Utilisez 'In' pour inclure plusieurs accountIds
            .EqualTo(x => x.ParentState, "posted")
            .EqualTo(x => x.Reconciled, false)
            .LessThanOrEqual(x => x.Date, prmDate); // Assurez-vous que Date est correctement mappé

        // Récupérez les lignes comptables correspondant au filtre
        var accountMoveLines = await _odooRepository.Query()
            .Where(filter)
            .ToListAsync();

        // Vérifiez si des lignes ont été trouvées
        if (accountMoveLines.Value == null || !accountMoveLines.Value.Any())
        {
            // Vous pouvez retourner une liste vide si une exception n'est pas nécessaire
            return new List<AccountMoveLineOdooModel>();
        }

        return accountMoveLines.Value;
    }
    catch (HttpRequestException ex)
    {
        // Gère les erreurs de réseau ou de communication avec Odoo
        throw new InvalidOperationException("Erreur de communication avec le serveur Odoo. Veuillez vérifier la connexion réseau.", ex);
    }
    catch (TimeoutException ex)
    {
        // Gère les cas de timeout
        throw new InvalidOperationException("La requête a expiré. Le serveur Odoo est peut-être surchargé ou indisponible.", ex);
    }

    catch (Exception ex)
    {
        // Vérifiez explicitement si c'est un OdooException
        if (ex.GetType().Name == "OdooException")
        {
            // Encapsulation dans WrappedOdooException
            throw new WrappedOdooException(ex);
        }
        
        // Gère toutes les autres exceptions
        throw new InvalidOperationException("Une erreur inattendue est survenue lors de la récupération des lignes comptables.", ex);
    }
}


}