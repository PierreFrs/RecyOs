using System.Collections.Generic;
using System.Linq;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Filters.Extensions.hub;

public static class EntrepriseBaseQueryExtension
{
    public static IQueryable<EntrepriseBase> ApplyFilter<TEntrepriseBase>(
        this IQueryable<TEntrepriseBase> query, EntrepriseBaseGridFilter filter)
        where TEntrepriseBase : EntrepriseBase, new()
    {
        // map dictionary for Materiau object
        var mapDictionary = new Dictionary<string, string>
        {
            {"Siren", "Siren"},
        };
        
        if (!string.IsNullOrWhiteSpace(filter.FiltredBySiren))
        {
            query = query.Where(x => x.Siren.Contains(filter.FiltredBySiren));
        }

        if (!string.IsNullOrWhiteSpace(filter.SortBy) && !string.IsNullOrWhiteSpace(filter.OrderBy))
        {
            query = query.OrderByDynamic(mapDictionary[filter.SortBy], filter.OrderBy == "ASC");
        }
        else
        {
            query = query.OrderByDynamic("Siren", true);
        }
        
        return query;
    }
}