// /** ICurrentContextProvider.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces;

public interface ICurrentContextProvider
{
    ContextSession GetCurrentContext();
}