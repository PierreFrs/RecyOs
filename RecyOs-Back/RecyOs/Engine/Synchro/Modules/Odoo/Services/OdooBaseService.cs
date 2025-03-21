//  OdooBaseService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using RecyOs.Engine.Services;

namespace RecyOs.Engine.Modules.Odoo.Services;

public abstract class OdooBaseService<TDestination> : BaseDataService<TDestination> where TDestination : class
{
    protected OdooBaseService() : base()
    {
        
    }
}