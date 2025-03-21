// ======================================================================0
// Crée par : brollin
// Fichier Crée le : 17/02/2025
// Fichier Modifié le : 17/02/2025
// Code développé pour le projet : RecyOs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Filters.Extensions.hub;

public static class BalanceParticulierQueryExtension
{
    public static IQueryable<BalanceParticulier> ApplyFilter<TBalanceParticulier>(this IQueryable<TBalanceParticulier> query, BalanceParticulierGridFilter filter)
    where TBalanceParticulier : BalanceParticulier, new()
    {
        var mapDictionary = new Dictionary<string, string>
        {
            {"clientId", "ClientId"},
            {"societeId", "SocieteId"},
            {"montant", "Montant"},
            {"dateRecuperationBalance", "DateRecuperationBalance"},
        };

        if (filter == null)
        {
            return query;
        }
        if (!string.IsNullOrEmpty(filter.FilterByClientId) && int.TryParse(filter.FilterByClientId, out int clientId))
        {
            query = query.Where(b => b.ClientId == clientId);
        }
        if (!string.IsNullOrWhiteSpace(filter.FilterClientName))
        {
            query = query.Where(b => b.ClientParticuliers.Nom.Contains(filter.FilterClientName));
        }
        if (!string.IsNullOrWhiteSpace(filter.FilterBySocieteId) && int.TryParse(filter.FilterBySocieteId, out int societeId))
        {
            query = query.Where(b => b.SocieteId == societeId);
        }
        if (!string.IsNullOrWhiteSpace(filter.SortBy) && !string.IsNullOrWhiteSpace(filter.OrderBy))
        {
            query = query.OrderByDynamic(mapDictionary[filter.SortBy], filter.OrderBy == "desc");
        }
        else
        {
            query = query.OrderByDescending(obj => obj.Montant);
        }
        return query;
    }
}
    