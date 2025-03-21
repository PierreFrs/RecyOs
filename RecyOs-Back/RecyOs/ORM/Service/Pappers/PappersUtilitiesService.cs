//  PappersUtilitiesService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 12/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service.hub;
using NLog;
using RecyOs.Controllers;
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service.pappers;

public class PappersUtilitiesService: IPappersUtilitiesService
{
    protected readonly IPappersApiService _pappersAPIService;
    private readonly Func<IEtablissementClientService> _etablissementClientServiceFactory;
    private readonly Func<IEtablissementFournisseurService> _etablissementFournisseurServiceFactory;
    protected readonly IEntrepriseBaseService _entrepriseBaseService;
    protected readonly IEtablissementFicheService _etablissementFicheService;
    private readonly ITokenInfoService _tokenInfoService;
    private readonly ILogger<PappersUtilitiesService> _logger;

    
    public PappersUtilitiesService(
        IPappersApiService pappersAPIService,
        Func<IEtablissementClientService> etablissementClientServiceFactory,
        Func<IEtablissementFournisseurService> etablissementFournisseurServiceFactory,
        IEntrepriseBaseService entrepriseBaseService,
        IEtablissementFicheService etablissementFicheService,
        ITokenInfoService tokenInfoService, 
        ILogger<PappersUtilitiesService> logger
        )
    {
        _pappersAPIService = pappersAPIService;
        _etablissementClientServiceFactory = etablissementClientServiceFactory;
        _etablissementFournisseurServiceFactory = etablissementFournisseurServiceFactory;
        _entrepriseBaseService = entrepriseBaseService;
        _etablissementFicheService = etablissementFicheService;
        _tokenInfoService = tokenInfoService;
        _logger = logger;
    }

    /// <summary>
    /// Vérifie si un numéro SIRET est valide.
    /// </summary>
    /// <param name="siret">Le numéro SIRET à vérifier.</param>
    /// <returns>Retourne true si le numéro SIRET est valide, sinon retourne false.</returns>
    public bool CheckSiret(string siret)
    {
        if (siret.Length != 14 || !Regex.IsMatch(siret, @"^\d{14}$"))
        {
            return false;
        }

        int sum = 0;

        for (int i = 0; i < siret.Length; i++)
        {
            int digit = int.Parse(siret[i].ToString());

            if (i % 2 == 0)
            {
                digit *= 2;

                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
        }

        return sum % 10 == 0;
    }

    /// <summary>
    /// Crée une fiche etablissement à partir d'un siret
    /// Créé également une fiche EtablissementClient avec les valeurs par défaut
    /// Crée également une fiche entreprise si elle n'existe pas
    /// Si les données de l'etablissement ne sont pas difusibles, on crée les fiche avec les données minimales
    /// </summary>
    /// <param name="siret">Le numéro SIRET de l'établissement à ajouter.</param>
    /// <param name="estClient"></param>
    /// <param name="estFournisseur"></param>
    /// <param name="disableTracking"></param>
    /// <returns id="etablissementClient">Retourne l'etablissementClient créé.</returns>
    public async Task<EtablissementClientDto> CreateEtablissementClientBySiret(string siret , bool estClient, bool estFournisseur, bool disableTracking = false)
    {
        // On vérifie que le siret est valide
        if (!CheckSiret(siret))
        {
            return null;
        }
        
        // On récupère les données de l'etablissement
        var etablissement = await _pappersAPIService.GetEtablissement(siret);
        // Si l'etablissementClient n'existe pas sur pappers, on retourne une erreur
        if (etablissement == null)
        {
            return null;
        }
        else
        {
            EntrepriseBaseDto entrepriseBase = new EntrepriseBaseDto(etablissement, _tokenInfoService.GetCurrentUserName());
            // Si les données de l'etablissement ne sont pas difusibles, on crée les fiche avec les données minimales
            if (!entrepriseBase.Diffusable)
            {
                _logger.LogInformation("Les données de l'etablissement ne sont pas difusibles");
                // Appel de la procédure pour créer l'etablissement non diffusable
                return await CreateEtablissementClientNonDiffusableBySiret(siret, estClient, estFournisseur);
            }
            else
            {   // On crée la fiche entreprise
                _logger.LogInformation("Création de la fiche entreprise avec les données de pappers");
                EtablissementFicheDto etablissementFiche = new EtablissementFicheDto(etablissement, _tokenInfoService.GetCurrentUserName());
                EtablissementClientDto etablissementClient = new EtablissementClientDto(entrepriseBase, 
                    etablissementFiche, estClient, estFournisseur);

                var etablissementClientService = _etablissementClientServiceFactory();
                var existingClient = await etablissementClientService.GetBySiret(siret, true);
                if (existingClient != null)
                {
                    etablissementClient.Client = existingClient.Client;
                    etablissementClient.Fournisseur = existingClient.Fournisseur;
                }

                if (estClient)
                {
                    etablissementClient.Client = true;
                    etablissementClient = await etablissementClientService.Create(etablissementClient, entrepriseBase, etablissementFiche, true);

                    return await etablissementClientService.Edit(etablissementClient);
                }
                else if (estFournisseur)
                {
                    etablissementClient.Fournisseur = true;
                    var etablissementFournisseurService = _etablissementFournisseurServiceFactory();
                    return await etablissementFournisseurService.Create(etablissementClient, entrepriseBase, etablissementFiche, false);
                }
            }
        }
        throw new InvalidOperationException("An etablissement must be either a client or a fournisseur.");
    }
    
    /* <summary>
     *  Permet de créer une entreprise non difusible à partir d'un siret 
     * </summary>
     * <param name="siret">Le numéro SIRET de l'établissement à ajouter.</param>
     * <returns id="etablissementClient">Retourne l'etablissementClient créé.</returns>
     */
    private async Task<EtablissementClientDto> CreateEtablissementClientNonDiffusableBySiret(string siret, 
        bool estClient, bool estFournisseur)
    {
        // On vérifie que le siret est valide
        if (!CheckSiret(siret))
        {
            return null;
        }

        EntrepriseBaseDto entrepriseBase = new EntrepriseBaseDto()
        {
            Siren = siret.Substring(0, 9),
            Diffusable = false,
        };
        await _entrepriseBaseService.Create(entrepriseBase);
        
        EtablissementClientDto etablissementClient = new EtablissementClientDto(siret, estClient, estFournisseur);
        if (estClient)
        {
            var _etablissementClientService = _etablissementClientServiceFactory();
            return await _etablissementClientService.Create(etablissementClient);
        }
        else if (estFournisseur)
        {
            var _etablissementFournisseurService = _etablissementFournisseurServiceFactory();
            return await _etablissementFournisseurService.Create(etablissementClient);
        }
        throw new InvalidOperationException("An etablissement must be either a client or a fournisseur.");
    }
    
    /* <summary>
     *  Permet de mettre à jour un etablissement par son siret 
     * </summary>
     * <param name="siret">Le numéro SIRET de l'établissement à mettre à jour.</param>
     * <returns result="bool">Retourne true si l'etablissement a été mis à jour, sinon retourne false.</returns>
     */
    public async Task<bool?> UpdateEtablissementClientBySiret(string siret)
    {
        // On vérifie que le siret est valide
        if (!CheckSiret(siret))
        {
            return null;
        }

        // On récupère les données de l'etablissement
        var etablissement = await _pappersAPIService.GetEtablissement(siret);
        // Si l'etablissementClient n'existe pas sur pappers, on retourne une erreur
        if (etablissement == null)
        {
            return null;
        }
        else
        {
            EntrepriseBaseDto entrepriseBase = new EntrepriseBaseDto(etablissement, _tokenInfoService.GetCurrentUserName());
            if (!entrepriseBase.Diffusable)
            {
                _logger.LogInformation("Les données de l'etablissement ne sont pas difusibles");
                return false;
            }
            else
            {
                EtablissementFicheDto etablissementFiche =
                    new EtablissementFicheDto(etablissement, _tokenInfoService.GetCurrentUserName());
                await _entrepriseBaseService.Edit(entrepriseBase);
                await _etablissementFicheService.Edit(etablissementFiche);
                return true;
            }
        }
    }
    
#nullable enable
    /* <summary>
     *  Permet de créer un etablissement par son siret
     * </summary>
     * <param name="siret">Le numéro SIRET de l'établissement à mettre à jour.</param>
     * <returns result="bool">Retourne true si l'etablissement a été crée, sinon retourne false.</returns>
     */
    public async Task<EtablissementFicheDto?> CreateEntrepriseBySiret(string siret)
    {
        // On vérifie que le siret est valide
        if (!CheckSiret(siret))
        {
            return null;
        }

        // On récupère les données de l'etablissement
        var etablissement = await _pappersAPIService.GetEtablissement(siret);
        // Si l'etablissementClient n'existe pas sur pappers, on retourne une erreur
        if (etablissement == null)
        {
            return null;
        }
        else
        {
            EntrepriseBaseDto entrepriseBase = new EntrepriseBaseDto(etablissement, _tokenInfoService.GetCurrentUserName());
            if (!entrepriseBase.Diffusable)
            {
                _logger.LogInformation("Les données de l'etablissement ne sont pas difusibles");
                entrepriseBase.Siren = siret.Substring(0, 9);
                await _entrepriseBaseService.Create(entrepriseBase);
                return null;
            }
            else
            {
                EtablissementFicheDto etablissementFiche =
                    new EtablissementFicheDto(etablissement, _tokenInfoService.GetCurrentUserName());
                await _entrepriseBaseService.Create(entrepriseBase);
                await _etablissementFicheService.Create(etablissementFiche);
                return etablissementFiche;
            }
        }
    }
#nullable disable
}