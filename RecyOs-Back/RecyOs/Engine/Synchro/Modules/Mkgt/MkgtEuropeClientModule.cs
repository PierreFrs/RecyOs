//  MkgtClientModule.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 04/01/2024
// Code développé pour le projet : RecyOs
using AutoMapper;
using Microsoft.Extensions.Logging;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.Engine.Modules;

public class MkgtEuropeClientModule: BaseModule<ClientEuropeDto, EtablissementMkgtDto>
{
    public MkgtEuropeClientModule(IMkgtClientService prmDataService, IEngineEuropeClientService hubService,
        IMapper mapper, IEngineSyncStatusService prmEngineSyncStatusService, ILogger<MkgtEuropeClientModule> logger) : 
        base("MkgtEuropeClientModule", prmDataService, hubService, mapper, prmEngineSyncStatusService, logger)
    {
    }
}