// /** EtablissementClientService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 21/03/2023
//  * Fichier Modifié le : 25/02/2024
//  * Code développé pour le projet : RecyOs.EtablissementClientService
//  */

using AutoMapper;
using RecyOs.MKGT_DB.Entities;
using RecyOs.MKGT_DB.Interfaces;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.Service.hub;

public class EtablissementClientService<TEtablissementClient> : 
    EtablissementService<TEtablissementClient, 
    IEtablissementClientRepository<TEtablissementClient>>,
    IEtablissementClientService
    where TEtablissementClient : EtablissementClient, new()
{
    public EtablissementClientService(
        ICurrentContextProvider contextProvider, 
        IEtablissementClientRepository<TEtablissementClient> etablissementClientRepository, 
        IEtablissementFicheRepository<EtablissementFiche> etablissementFicheRepository,
        IFCliRepository<Fcli> fCliRepository,
        IPappersUtilitiesService pappersUtilitiesService,
        IEtablissementServiceUtilitaryMethods etablissementServiceUtilitaryMethods,
        ITokenInfoService tokenInfoService,
        IMapper mapper,
        IGroupRepository groupRepository) 
        : base(
            contextProvider, 
            etablissementClientRepository,
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