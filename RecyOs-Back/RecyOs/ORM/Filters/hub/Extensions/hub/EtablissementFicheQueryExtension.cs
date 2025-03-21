using System.Collections.Generic;
using System.Linq;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Filters.Extensions.hub;

public static class EtablissementFicheQueryExtension
{
    public static IQueryable<EtablissementFiche> ApplyFilter<TEtablissementFiche>(
        this IQueryable<TEtablissementFiche> query, EtablissementFicheGridFilter filter)
        where TEtablissementFiche : EtablissementFiche, new()
    {
        // map dictionary for EtablissementFiche object
        var mapDictionary = new Dictionary<string, string>
        {
            {"Siret", "Siret"}
        };
        
        if (!string.IsNullOrWhiteSpace(filter.FiltredBySiret))
        {
            query = query.Where(x => x.Siret.Contains(filter.FiltredBySiret));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Radies))
        {
            query = query.Where(x => x.EtablissementCesse == true);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.SortBy) && !string.IsNullOrWhiteSpace(filter.OrderBy))
        {
            query = query.OrderByDynamic(mapDictionary[filter.SortBy], filter.OrderBy == "ASC");
        }
        else
        {
            query = query.OrderByDynamic("Siret", true);
        }
        
        return query;
    }

}