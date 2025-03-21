//  MkgtClientModule.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.Engine.Modules;

public class MkgtClientModule: BaseModule<EtablissementClientDto, EtablissementMkgtDto>
{
    public MkgtClientModule(IMkgtClientService prmDataService, IEngineEtablissementClientService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<MkgtClientModule> logger) : 
        base("MkgtClientModule", prmDataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
    }
}