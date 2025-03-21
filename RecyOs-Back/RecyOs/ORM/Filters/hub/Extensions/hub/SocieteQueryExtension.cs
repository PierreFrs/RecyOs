using System.Collections.Generic;
using System.Linq;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Filters.Extensions.hub;

public static class SocieteQueryExtension
{
    public static IQueryable<Societe> ApplyFilter(this IQueryable<Societe> query, SocieteGridFilter filter)
    {
        var mapDictionary = new Dictionary<string, string>
        {
            {"Id", "Id"},
            {"Nom", "Nom"},
            {"IdOdoo", "IdOdoo"}
        };

        if (!string.IsNullOrWhiteSpace(filter.FilterBySocieteId) && int.TryParse(filter.FilterBySocieteId, out int societeId))
        {
            query = query.Where(s => s.Id == societeId);
        }
        if (!string.IsNullOrWhiteSpace(filter.FilterByNom))
        {
            query = query.Where(s => s.Nom.Contains(filter.FilterByNom));
        }
        if (!string.IsNullOrWhiteSpace(filter.FilterByIdOdoo))
        {
            query = query.Where(s => s.IdOdoo.Contains(filter.FilterByIdOdoo));
        }
        if (!string.IsNullOrWhiteSpace(filter.FilterIsDeleted))
        {
            query = query.Where(s => s.IsDeleted == bool.Parse(filter.FilterIsDeleted));
        }
        else if (!string.IsNullOrWhiteSpace(filter.SortBy) && mapDictionary.ContainsKey(filter.SortBy))
        {
            query = query.OrderByDynamic(mapDictionary[filter.SortBy], filter.OrderBy == "desc");
        }
        else
        {
            query = query.OrderByDescending(s => s.Nom);
        }
        return query;
    }
}
