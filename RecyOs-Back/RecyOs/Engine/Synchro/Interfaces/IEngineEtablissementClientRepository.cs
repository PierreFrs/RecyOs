//  IEtablissementClientRepository.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 30/11/2024
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Engine.Interfaces;

public interface IEngineEtablissementClientRepository
{
    public IList<EtablissementClient> GetUpdatedEntities(string moduleName);
    public IList<EtablissementClient> GetCreatedEntities(string moduleName);
    Task<string> GetMontantGarantieForEntreprise(string siren);
    public IList<EtablissementClient> CallBackDestIdCreation(string moduleName, IList<EtablissementClient> items);
}