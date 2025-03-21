// /** FuseNavigationMenuExtension.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/05/2023
//  * Fichier Modifié le : 23/05/2023
//  * Code développé pour le projet : Fuse Framework
//  */
using System.Collections.Generic;
using System.Linq;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Filters.Extensions;

public static class FuseNavigationMenuExtension
{
    public static IQueryable<FuseNavigationItem> ApplyFilter<TFuseNavigationMenu>(this IQueryable<TFuseNavigationMenu> query, FuseNavigationMenuFilter filter)
        where TFuseNavigationMenu : FuseNavigationItem, new()
    {
        // map dictionary for Origine object
        var mapDictionary = new Dictionary<string, string>
        {
            {"Title", "Title"},
        };

        if (!string.IsNullOrWhiteSpace(filter.FiltredByTitle))
        {
            query = query.Where(x => x.Title.ToLower().Contains(filter.FiltredByTitle.ToLower()));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.SortBy) && !string.IsNullOrWhiteSpace(filter.OrderBy))
        {
            query = query.OrderByDynamic(mapDictionary[filter.SortBy], filter.OrderBy == "ASC");
        }
        else
        {
            query = query.OrderByDescending(obj => obj.Title);
        }
        return query;
    }
}