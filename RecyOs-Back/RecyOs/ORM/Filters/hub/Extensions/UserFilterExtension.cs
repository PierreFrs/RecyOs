// /** UserFilterExtension.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 22/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System.Collections.Generic;
using System.Linq;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Filters.Extensions;


public static class UserFilterExtension
{
    public static IQueryable<User> ApplyFilter<TUser>(this IQueryable<TUser> query, UsersGridFilter filter)
        where TUser : User, new()
    {
        // map dictionary for User object
        var mapDictionary = new Dictionary<string, string>
        {
            {"firstName", "FirstName"},
            {"lastName", "LastName"},
            {"Username", "UserName"},
            {"email", "Email"},
        };

        if (!string.IsNullOrWhiteSpace(filter.FilterByFirstName))
        {
            query = query.Where(obj => obj.FirstName.ToLower().Contains(filter.FilterByFirstName.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.FilterByLastName))
        {
            query = query.Where(obj => obj.LastName.ToLower().Contains(filter.FilterByLastName.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.FilterByLogin))
        {
            query = query.Where(obj => obj.UserName.ToLower().Contains(filter.FilterByLogin.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.FilterByEmail))
        {
            query = query.Where(obj => obj.Email.ToLower().Contains(filter.FilterByEmail.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.SortBy) && !string.IsNullOrWhiteSpace(filter.OrderBy))
        {
            query = query.OrderByDynamic(mapDictionary[filter.SortBy], filter.OrderBy == "DESC");
        }
        else
        {
            query = query.OrderByDescending(obj => obj.UserName);
        }

        return query;
    }
}
