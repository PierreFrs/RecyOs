
// /** BaseFilter.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 22/01/2021
//  * Code développé pour le projet : Archimede.Common
//  */
namespace RecyOs.ORM.Filters;

public class BaseFilter
{
    public BaseFilter(int pageNumber = 1, int pageSize = 1000)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string SortBy { get; set; }
    public string OrderBy { get; set; }
}