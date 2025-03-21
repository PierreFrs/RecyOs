// ClientParticulierQueryExtension.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 08/10/2024
// Fichier Modifié le : 08/10/2024
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Filters.Extensions.hub;

public static class ClientParticulierQueryExtension
{
    public static IQueryable<ClientParticulier> ApplyFilter(
        this IQueryable<ClientParticulier> query, ClientParticulierGridFilter filter)
    {
        // map dictionary for EtablissementClient object
        var mapDictionary = new Dictionary<string, string>
        {
            {"CodeMkgt", "CodeMkgt"},
            {"IdOdoo", "IdOdoo"},
            {"Nom", "Nom"}
        };
        
        if (!string.IsNullOrWhiteSpace(filter.FilterByCodeMkgt))
        {
            query = query.Where(x => x.CodeMkgt.Contains(filter.FilterByCodeMkgt));
        }
        if (!string.IsNullOrWhiteSpace(filter.FilterByIdOdoo))
        {
            query = query.Where(x => x.IdOdoo.Contains(filter.FilterByIdOdoo));
        }
        if (!string.IsNullOrWhiteSpace(filter.FilterByNom))
        {
            query = query.Where(x => x.Nom.Contains(filter.FilterByNom));
        }
        if (!string.IsNullOrWhiteSpace(filter.BadMail))
        {
            query = query.Where(x => x.EmailFacturation == null);
        }
        if (!string.IsNullOrWhiteSpace(filter.BadTel))
        {
            query = query.Where(x => x.TelephoneFacturation == null);
        }
        else if (!string.IsNullOrWhiteSpace(filter.SortBy) && mapDictionary.ContainsKey(filter.SortBy))
        {
            query = query.OrderByDynamic(mapDictionary[filter.SortBy], filter.OrderBy == "desc");
        }
        else
        {
            query = query.OrderByDescending(obj => obj.Nom);
        }

        return query;
    }
}