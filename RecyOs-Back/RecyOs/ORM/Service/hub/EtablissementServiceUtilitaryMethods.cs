// EtablissementServiceUtilitaryMethods.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 05/08/2024
// Fichier Modifié le : 05/08/2024
// Code développé pour le projet : RecyOs

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NLog;
using RecyOs.MKGT_DB.Entities;
using RecyOs.MKGT_DB.Interfaces;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.Service.hub;

public class EtablissementServiceUtilitaryMethods : IEtablissementServiceUtilitaryMethods
{
    private readonly IEtablissementRepository<EtablissementClient> _etablissementRepository;
    private readonly IEtablissementFicheRepository<EtablissementFiche> _etablissementFicheRepository;
    private readonly IPappersUtilitiesService _pappersUtilitiesService;
    private readonly IFCliRepository<Fcli> _fcliRepository;
    private readonly IFactorClientFranceBuRepository _factorClientFranceBuRepository;
    private readonly IDocumentPdfRepository<DocumentPdf> _documentPdfRepository;
    private readonly IBalanceFranceRepository _balanceRepository;
    private readonly IEtablissementClientBusinessUnitRepository<EtablissementClientBusinessUnit, BusinessUnit> _etablissementClientBusinessUnitRepository;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    
    
    public EtablissementServiceUtilitaryMethods(
        IEtablissementRepository<EtablissementClient> etablissementRepository,
        IEtablissementFicheRepository<EtablissementFiche> etablissementFicheRepository,
        IPappersUtilitiesService pappersUtilitiesService,
        IFCliRepository<Fcli> fcliRepository,
        IFactorClientFranceBuRepository factorClientFranceBuRepository,
        IDocumentPdfRepository<DocumentPdf> documentPdfRepository,
        IBalanceFranceRepository balanceRepository,
        IEtablissementClientBusinessUnitRepository<EtablissementClientBusinessUnit, BusinessUnit> etablissementClientBusinessUnitRepository)
    {
        _etablissementRepository = etablissementRepository;
        _etablissementFicheRepository = etablissementFicheRepository;
        _pappersUtilitiesService = pappersUtilitiesService;
        _fcliRepository = fcliRepository;
        _factorClientFranceBuRepository = factorClientFranceBuRepository;
        _documentPdfRepository = documentPdfRepository;
        _balanceRepository = balanceRepository;
        _etablissementClientBusinessUnitRepository = etablissementClientBusinessUnitRepository;
    }
    
    public async Task<ServiceResult> EntityChecks(int id, string newSiret, ContextSession session)
    {
        _logger.Info($"EtablissementServiceUtilitaryMethods - EntityChecks - id: {id} - newSiret: {newSiret}");
        try
        {
            if (!_pappersUtilitiesService.CheckSiret(newSiret))
            {
                _logger.Error($"EtablissementServiceUtilitaryMethods - EntityChecks - SIRET Invalide");
                return BadRequest("SIRET Invalide");
            }
        
            var etablissementClient = await _etablissementRepository.GetById(id, session);
            if (etablissementClient == null)
            {
                _logger.Error($"EtablissementServiceUtilitaryMethods - EntityChecks - Etablissement client introuvable");
                return EntityNotFound("Etablissement client");
            }

            var etablissementFiche = await _etablissementFicheRepository.GetBySiret(etablissementClient.Siret, session);
            if (etablissementFiche == null)
            {
                _logger.Error($"EtablissementServiceUtilitaryMethods - EntityChecks - Etablissement fiche introuvable");
                return EntityNotFound("Etablissement fiche");
            }

            var fcli = new Fcli();
            if (etablissementClient.CodeMkgt != null)
            {
                fcli = await _fcliRepository.GetByCode(etablissementClient.CodeMkgt);
                if (fcli == null)
                {
                    _logger.Error($"EtablissementServiceUtilitaryMethods - EntityChecks - Fiche MKGT introuvable");
                    return EntityNotFound("Fiche MKGT");
                }
            }

            var checkEtablissementClient = await _etablissementRepository.GetBySiret(newSiret, session);
            if (checkEtablissementClient != null)
            {
                _logger.Error($"EtablissementServiceUtilitaryMethods - EntityChecks - Le SIRET existe déjà");
                return BadRequest("Le SIRET existe déjà");
            }

            return new ServiceResult
            {
                Success = true,
                Data = new SiretUpdateEntitiesListResult 
                { 
                    EtablissementFiche = etablissementFiche,
                    EtablissementClient = etablissementClient,
                    Fcli = fcli 
                }
            };
        }
        catch (Exception e)
        {
            _logger.Error($"EtablissementServiceUtilitaryMethods - EntityChecks - Erreur: {e.Message}");
            return new ServiceResult
            {
                Message = $"Une erreur est survenue lors de l'exécution: {e.Message}"
            };
        }

    }

    public async Task<ServiceResult> UpdateIdsInDependantEntities(int oldEtablissementClientId, int newEtablissementId, ContextSession session)
    {
        try
        {
            var factorClientResults = await _factorClientFranceBuRepository.UpdateClientIdInFactorClientFranceBuAsync(oldEtablissementClientId, newEtablissementId, session);
            if (!factorClientResults.Success)
            {
                return factorClientResults;
            }
        
            var documentPdfResults = await _documentPdfRepository.UpdateClientIdInDocumentPdfsAsync(oldEtablissementClientId, newEtablissementId, session);
            if (!documentPdfResults.Success)
            {
                return documentPdfResults;
            }
        
            var balanceResults = await _balanceRepository.UpdateClientIdInBalancesAsync(oldEtablissementClientId, newEtablissementId, session);
            if (!balanceResults.Success)
            {
                return balanceResults;
            }
        
            var buResults = await _etablissementClientBusinessUnitRepository.UpdateClientIdInBUsAsync(oldEtablissementClientId, newEtablissementId, session);
            if (!buResults.Success)
            {
                return buResults;
            }
        
            return new ServiceResult
            {
                Success = true,
                StatusCode = 200,
                Message = "Les ids ont été mis à jour"
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in UpdateIdsInDependantEntities: {e.Message}");
            return new ServiceResult
            {
                Message = $"Une erreur est survenue lors de l'exécution: {e.Message}"
            };
        }
    }

    private ServiceResult EntityNotFound(string entity)
    {
        return new ServiceResult
        {
            StatusCode = StatusCodes.Status404NotFound,
            Message = $"{entity} introuvable"
        };
    }
    
    private ServiceResult BadRequest(string message)
    {
        return new ServiceResult
        {
            Message = message
        };
    }
}