// /** MessageFilter.cs - Définition du filtre Message
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 03/02/2025
//  * Fichier Modifié le : 03/02/2025
//  * Code développé pour le projet : Fuse Framework
//  */

using RecyOs.ORM.Filters;

namespace RecyOs.ORM.Filters;

public class NotificationFilter : BaseFilter
{
    public string FiltredByTitle { get; set; }
    public bool? FiltredByRead { get; set; }
}