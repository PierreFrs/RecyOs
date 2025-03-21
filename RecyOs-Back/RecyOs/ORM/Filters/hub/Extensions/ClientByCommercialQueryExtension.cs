using System.Collections.Generic;
using System.Linq;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Filters.Extensions;

static class ClientByCommercialQueryExtension
{
    public static IQueryable<T> ApplyClientByCommercialSearchFilter<T>(
        this IQueryable<T> query, ClientByCommercialFilter filter)
    where T : class, ICommercialClient
    {
        var mapDictionary = new Dictionary<string, string>
        {
            { "Nom", "Nom" },
            { "Ville", "VilleFacturation"},
            { "Identifiant", "Identifiant" },
            { "IdOdoo", "IdOdoo" },
            { "CodeMkgt", "CodeMkgt" },
            { "CodeGpi", "CodeGpi" },
        };

        if (!string.IsNullOrWhiteSpace(filter.SearchByNom))
        {
            query = query.Where(x => x.Nom.Contains(filter.SearchByNom));
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchByIdentifiant))
        {
            // Filter by Identifiant, requires type-specific logic
            // This part needs to be done differently since you can't directly filter by Identifiant in a generic context.
            query = ApplyIdentifiantFilter(query, filter.SearchByIdentifiant);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchByCodeMkgt))
        {
            query = query.Where(x => x.CodeMkgt.Contains(filter.SearchByCodeMkgt));
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchByIdOdoo))
        {
            query = query.Where(x => x.IdOdoo.Contains(filter.SearchByIdOdoo));
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchByCodeGpi))
        {
            query = query.Where(x => x.CodeGpi.Contains(filter.SearchByCodeGpi));
        }

        if (!string.IsNullOrWhiteSpace(filter.SortBy) && mapDictionary.ContainsKey(filter.SortBy))
        {
            bool isDescending = filter.OrderBy == "desc";
            query = query.OrderByDynamic(mapDictionary[filter.SortBy], isDescending);
        }
        else
        {
            query = query.OrderByDescending(x => x.Nom);
        }

        return query;
    }
    
    public static IQueryable<T> ApplyClientByCommercialUnitFilter<T>(
        this IQueryable<T> query, ClientByCommercialFilter filter)
    where T : class, ICommercialClient
    {
        if (filter.FilterByMkgt)
        {
            query = query.Where(x => x.CodeMkgt != null);
        }

        if (filter.FilterByGpi)
        {
            query = query.Where(x => x.CodeGpi != null);
        }

        return query;
    }

    private static IQueryable<T> ApplyIdentifiantFilter<T>(IQueryable<T> query, string searchByIdentifiant) where T : class, ICommercialClient
    {
        // Example for handling specific types, adjust as necessary for your implementation
        if (typeof(T) == typeof(EtablissementClient))
        {
            return (query as IQueryable<EtablissementClient>)
                .Where(x => x.Siret.Contains(searchByIdentifiant))
                .Cast<T>();
        }
        else if (typeof(T) == typeof(ClientEurope))
        {
            return (query as IQueryable<ClientEurope>)
                .Where(x => x.Vat.Contains(searchByIdentifiant))
                .Cast<T>();
        }
        
        return query;
    }
}
