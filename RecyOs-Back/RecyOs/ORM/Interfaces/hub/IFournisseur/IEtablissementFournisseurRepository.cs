// /** IEtablissementFournisseurRepository.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 15/02/2024
//  * Fichier Modifié le : 25/02/2024
//  * Code développé pour le projet : RecyOs.EtablissementFournisseurService
//  */

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEtablissementFournisseurRepository<TEtablissementClient> : IEtablissementRepository<TEtablissementClient>
    where TEtablissementClient : EtablissementClient
{
    
}