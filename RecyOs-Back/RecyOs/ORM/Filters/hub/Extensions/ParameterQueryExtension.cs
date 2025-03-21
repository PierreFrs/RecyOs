using System.Collections.Generic;
using System.Linq;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Filters.Extensions;

public static class ParameterQueryExtension
{
    public static IQueryable<Parameter> ApplyFilter<TParameter>(
        this IQueryable<TParameter> query, ParameterFilter filter)
        where TParameter : Parameter, new()
    {
        var mapDictionary = new Dictionary<string, string>
        {
            {"module", "Module"},
            {"nom", "Nom"}
        };
        
        if (!string.IsNullOrEmpty(filter.FilteredByModule))
        {
            query = query.Where(x => x.Module.Contains(filter.FilteredByModule));
        }
        if (!string.IsNullOrEmpty(filter.FilteredByNom))
        {
            query = query.Where(x => x.Nom.Contains(filter.FilteredByNom));
        }
        else if (!string.IsNullOrWhiteSpace(filter.SortBy) && mapDictionary.ContainsKey(filter.SortBy))
        {
            string mappedSortBy = mapDictionary[filter.SortBy];
            bool isDescending = filter.OrderBy == "desc";
            query = query.OrderByDynamic(mappedSortBy, isDescending);
        }
        else
        {
            query = query.OrderByDescending(obj => obj.Nom);
        }

        return query;
    }
}