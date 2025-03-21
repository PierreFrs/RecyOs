using System.Collections.Generic;
using RecyOs.ORM.Entities.hub;
using System.Linq;

namespace RecyOs.ORM.Filters.Extensions;

public static class GroupFilterExtensions
{
    public static IQueryable<Group> ApplyFilter(this IQueryable<Group> query, GroupFilter filter)
    {
        var mapDictionary = new Dictionary<string, string>
        {
            {"Nom", "Name"},
            {"FicheCount", "FicheCount"}
        };

        if (!string.IsNullOrEmpty(filter.FilteredByNom))
        {
            query = query.Where(g => g.Name.Contains(filter.FilteredByNom));
        }

        // Handle sorting
        if (!string.IsNullOrWhiteSpace(filter.SortBy))
        {
            switch (filter.SortBy)
            {
                case "Nom":
                    query = filter.OrderBy == "asc" 
                        ? query.OrderBy(g => g.Name)
                        : query.OrderByDescending(g => g.Name);
                    break;
                
                case "ficheCount":
                    query = filter.OrderBy?.ToLower() == "asc"
                        ? query.OrderBy(g => g.EtablissementClients.Count + g.ClientEuropes.Count)
                        : query.OrderByDescending(g => g.EtablissementClients.Count + g.ClientEuropes.Count);
                    break;
                
                default:
                    if (mapDictionary.ContainsKey(filter.SortBy))
                    {
                        query = query.OrderByDynamic(mapDictionary[filter.SortBy], filter.OrderBy == "desc");
                    }
                    break;
            }
        }
        else
        {
            query = query.OrderByDescending(g => g.Name);
        }

        return query;
    }
}