using System;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NLog;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using ILogger = NLog.ILogger;

namespace RecyOs.ORM.Service.vatlayer;

public class VatlayerUtilitiesService : IVatlayerUtilitiesService
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    protected readonly VatlayerApiService _vatlayerAPIService;
    protected readonly IClientEuropeService _clientEuropeService;
    protected readonly IFournisseurEuropeService _fournisseurEuropeService;
    private readonly ITokenInfoService _tokenInfoService;
    
    public VatlayerUtilitiesService(VatlayerApiService vatlayerAPIService, 
        IClientEuropeService clientEuropeService,
        IFournisseurEuropeService fournisseurEuropeService,
        ITokenInfoService tokenInfoService,
        ILogger<VatlayerUtilitiesService> logger
        )
    {
        _vatlayerAPIService = vatlayerAPIService;
        _clientEuropeService = clientEuropeService;
        _fournisseurEuropeService = fournisseurEuropeService;
        _tokenInfoService = tokenInfoService;
    }
    
    /// <summary>
    /// Vérifie si un numéro de TVA est valide.
    /// </summary>
    /// <param name="vatNumber">Le numéro de TVA à vérifier.</param>
    /// <returns>Retourne true si le numéro de TVA est valide, sinon retourne false.</returns>
    public bool CheckVatNumber(string vatNumber)
    {
        
        var clientEurope =  _vatlayerAPIService.GetClientEurope(vatNumber);
        if (clientEurope == null)
        {
            Logger.Trace($"CheckVatNumber : {vatNumber} result : false");
            return false;
        }
        else
        {
            JsonElement root = JsonSerializer.Deserialize<JsonElement>(clientEurope.Result);
            bool valid = root.GetProperty("valid").GetBoolean();
            Logger.Trace($"CheckVatNumber : {vatNumber} result : {valid}");
            return valid;
        }
    }
    
    /// <summary>
    /// Permet de créer un clientEurope à partir d'un numéro de TVA.
    /// </summary>
    /// <param name="vatNumber">Le numéro de TVA du clientEurope.</param>
    /// <returns>Retourne le clientEurope créé.</returns>
    public async Task<ClientEuropeDto> CreateClientEurope(string vatNumber, bool estClient, bool estFournisseur, bool disableTracking)
    {
        var clientEurope =  await _vatlayerAPIService.GetClientEurope(vatNumber);
        if (clientEurope == null)
        {
            return null;
        }
        else
        {
            JsonElement root = JsonSerializer.Deserialize<JsonElement>(clientEurope);
            bool valid = root.GetProperty("valid").GetBoolean();
            if (valid)
            {
                var adr = new VatlayerAddrParse(root.GetProperty("company_address").GetString());
                ClientEuropeDto clientEuropeDto = new ClientEuropeDto();
                clientEuropeDto.Vat = root.GetProperty("query").GetString();
                clientEuropeDto.Nom = root.GetProperty("company_name").GetString();
                clientEuropeDto.PaysFacturation = CodeToPaysFacturation(root.GetProperty("country_code").GetString());
                clientEuropeDto.AdresseFacturation1 = adr.adr1?.ToUpper();
                clientEuropeDto.AdresseFacturation2 = adr.adr2?.ToUpper();
                clientEuropeDto.CodePostalFacturation = adr.cp;
                clientEuropeDto.VilleFacturation = adr.country?.ToUpper();
                clientEuropeDto.CreateDate = DateTime.Now.ToString();
                clientEuropeDto.ConditionReglement = 1;
                clientEuropeDto.ModeReglement = 4;
                clientEuropeDto.DelaiReglement = 0;
                clientEuropeDto.TauxTva = 0;
                clientEuropeDto.EncoursMax = 0;
                clientEuropeDto.CompteComptable = "411107";
                clientEuropeDto.FrnConditionReglement = 0;
                clientEuropeDto.FrnModeReglement = 0;
                clientEuropeDto.FrnDelaiReglement = 30;
                clientEuropeDto.FrnTauxTva = 00;
                clientEuropeDto.FrnEncoursMax = 1000;
                clientEuropeDto.FrnCompteComptable = "401106";
                clientEuropeDto.CreatedBy = _tokenInfoService.GetCurrentUserName();
                clientEuropeDto.Radie = !root.GetProperty("valid").GetBoolean();  // Si le clientEurope est valide, il n'est pas radié.
                clientEuropeDto.CategorieId = 1; // Default value for N/A category

                var existingClient = await _clientEuropeService.GetByVat(vatNumber, true);
                if (existingClient != null)
                {
                    clientEuropeDto.Client = existingClient.Client;
                    clientEuropeDto.Fournisseur = existingClient.Fournisseur;
                }

                if (estClient)
                {
                    clientEuropeDto.Client = true;
                    clientEuropeDto = await _clientEuropeService.Create(clientEuropeDto);

                    return await _clientEuropeService.Update(clientEuropeDto);
                }
                else if (estFournisseur)
                {
                    clientEuropeDto.Fournisseur = true;
                    return await _fournisseurEuropeService.Create(clientEuropeDto);
                }
            }
            else
            {
                return null;
            }
        }
        throw new InvalidOperationException("An etablissement must be either a client or a fournisseur.");
    }
    
    private static string CodeToPaysFacturation(string code)
    {
        switch (code.ToUpper())
        {
            case "BE":
                return "BELGIQUE";
            case "LU":
                return "LUXEMBOURG";
            case "CH":
                return "SUISSE";
            case "DE":
                return "ALLEMAGNE";
            case "ES":
                return "ESPAGNE";
            case "IT":
                return "ITALIE";
            case "PT":
                return "PORTUGAL";
            case "GB":
                return "ROYAUME-UNI";
            case "NL":
                return "PAYS-BAS";
            case "IE":
                return "IRLANDE";
            case "AT":
                return "AUTRICHE";
            case "BG":
                return "BULGARIE";
            case "CY":
                return "CHYPRE";
            case "HR":
                return "CROATIE";
            case "DK":
                return "DANEMARK";
            case "EE":
                return "ESTONIE";
            case "FI":
                return "FINLANDE";
            case "GR":
                return "GRECE";
            case "HU":
                return "HONGRIE";
            case "LV":
                return "LETTONIE";
            case "LT":
                return "LITUANIE";
            case "MT":
                return "MALTE";
            case "PL":
                return "POLOGNE";
            case "CZ":
                return "REPUBLIQUE TCHEQUE";
            case "RO":
                return "ROUMANIE";
            case "SK":
                return "SLOVAQUIE";
            case "SI":
                return "SLOVENIE";
            case "SE":
                return "SUEDE";
            default:
                return "INCONNU";
        }
    }

}

public class VatlayerAddrParse
{
    public readonly string adr1;
    public readonly string adr2;
    public readonly string cp;
    public readonly string country;

    public VatlayerAddrParse(string prmadress)
    {
        string[] lines = prmadress.Split('\n');
        adr1 = lines[0].Trim();
        if (lines.Length > 1)
        {
            string[] secondLineParts = lines[1].Trim().Split(' ');
            if (secondLineParts.Length > 1)
            {
                cp = secondLineParts[0];
                country = string.Join(" ", secondLineParts, 1, secondLineParts.Length - 1);
            }
            else
            {
                cp = lines[1].Trim();
            }
        }
        else
        {
            cp = "";
            country = "";
        }
    }
}