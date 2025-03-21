// /** UserGridFilter.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 22/01/2021
//  * Code développé pour le projet : Archimede.Common
//  */
namespace RecyOs.ORM.Filters;

public class UsersGridFilter : BaseFilter
{
    public string FilterByFirstName { get; set; }
    public string FilterByLastName { get; set; }
    public string FilterByLogin { get; set; }
    public string FilterByEmail { get; set; }

}