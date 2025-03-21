//  OdooClientModule.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.Odoo;
using RecyOs.OdooDB.DTO;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.Engine.Modules;

public class OdooClientModule: BaseModule<EtablissementClientDto, ResPartnerDto>
{
    public OdooClientModule(IOdooClientService prmdataService, IEngineEtablissementClientService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<OdooClientModule> logger) : 
        base("OdooClientModule", prmdataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
        
    }
}