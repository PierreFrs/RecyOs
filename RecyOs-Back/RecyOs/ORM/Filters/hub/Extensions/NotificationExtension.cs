// /** NotificationExtension.cs - Définition des extensions de Notification
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 03/02/2025
//  * Fichier Modifié le : 03/02/2025
//  * Code développé pour le projet : Fuse Framework
//  */

using System.Collections.Generic;
using System.Linq;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Filters.Extensions;

public static class NotificationExtension
{
    public static IQueryable<Notification> ApplyFilter<TNotification>(this IQueryable<TNotification> query, NotificationFilter filter)
        where TNotification : Notification, new()
    {
        var mapDictionary = new Dictionary<string, string>
        {
            {"Title", "Title"},
            {"Read", "Read"},
        };

        if (!string.IsNullOrWhiteSpace(filter.FiltredByTitle))
        {
            query = query.Where(x => x.Title.ToLower().Contains(filter.FiltredByTitle.ToLower()));
        }

        if (filter.FiltredByRead != null)
        {
            query = query.Where(x => x.Read == filter.FiltredByRead);
        }

        if (!string.IsNullOrWhiteSpace(filter.SortBy) && !string.IsNullOrWhiteSpace(filter.OrderBy))
        {
            query = query.OrderByDynamic(mapDictionary[filter.SortBy], filter.OrderBy == "ASC");
        }
        return query;
    }
}
