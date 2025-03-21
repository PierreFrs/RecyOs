// IEtablissementServiceUtilitaryMethods.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 05/08/2024
// Fichier Modifié le : 05/08/2024
// Code développé pour le projet : RecyOs

using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEtablissementServiceUtilitaryMethods
{
    Task<ServiceResult> EntityChecks(int id, string newSiret, ContextSession session);
    
    Task <ServiceResult> UpdateIdsInDependantEntities(int oldEtablissementClientId, int newEtablissementId, ContextSession session);
}