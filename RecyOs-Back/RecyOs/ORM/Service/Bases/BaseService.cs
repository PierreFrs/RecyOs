// /** BaseService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.Common
//  */

using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public abstract class BaseService
{
    protected ICurrentContextProvider contextProvider;
    protected readonly ContextSession Session;

    protected BaseService(ICurrentContextProvider contextProvider)
    {
        this.contextProvider = contextProvider;
        Session = contextProvider.GetCurrentContext();
    }
}