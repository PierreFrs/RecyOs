// /** EtablissementFournisseurService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 15/02/2024
//  * Fichier Modifié le : 15/02/2024
//  * Code développé pour le projet : RecyOs.EEtablissementFournisseurService
//  */

using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.MKGT_DB.Entities;
using RecyOs.MKGT_DB.Interfaces;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.Service.hub;

public class EtablissementFournisseurService<TEtablissementClient> : 
    EtablissementService<TEtablissementClient,
    IEtablissementFournisseurRepository<TEtablissementClient>>, 
    IEtablissementFournisseurService
    where TEtablissementClient : EtablissementClient, new()
{
    public EtablissementFournisseurService(
        ICurrentContextProvider contextProvider, 
        IEtablissementFournisseurRepository<TEtablissementClient> repository, 
        IEtablissementFicheRepository<EtablissementFiche> etablissementFicheRepository,
        IFCliRepository<Fcli> fCliRepository,
        IPappersUtilitiesService pappersUtilitiesService,
        IEtablissementServiceUtilitaryMethods etablissementServiceUtilitaryMethods,
        ITokenInfoService tokenInfoService,
        IMapper mapper,
        IGroupRepository groupRepository)
        : base(
            contextProvider, 
            repository, 
            etablissementFicheRepository,
            fCliRepository,
            pappersUtilitiesService, 
            etablissementServiceUtilitaryMethods,
            tokenInfoService,
            mapper,
            groupRepository)
    {
    }
    
}