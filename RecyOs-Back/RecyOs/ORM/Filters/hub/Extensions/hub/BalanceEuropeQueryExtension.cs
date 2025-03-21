// BalanceEuropeQueryExtension.cs
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

public static class BalanceEuropeQueryExtension
{
    public static IQueryable<BalanceEurope> ApplyFilter<TBalanceEurope>(this IQueryable<TBalanceEurope> query, BalanceEuropeGridFilter filter)
    where TBalanceEurope : BalanceEurope, new()
    {
        var mapDictionary = new Dictionary<string, string>
        {
            {"clientId", "ClientId"},
            {"cocieteId", "SocieteId"},
            {"clientCommercialId", "ClientEurope.CommercialId"},
            {"montant", "Montant"},
            {"dateRecuperationBalance", "DateRecuperationBalance"},
            {"etablissementClient.nom", "ClientEurope.Nom"},
            {"societe.nom", "Societe.Nom"},
            {"commercial", "ClientEurope.Commercial.Lastname"},
            {"encoursMax", "ClientEurope.EncoursMax"},
        };

        if (filter == null)
        {
            return query;
        }
        if (!string.IsNullOrEmpty(filter.FilterByClientId) && int.TryParse(filter.FilterByClientId, out int clientId))
        {
            query = query.Where(b => b.ClientId == clientId);
        }
        if (!string.IsNullOrEmpty(filter.FilterBySocieteId) && int.TryParse(filter.FilterBySocieteId, out int societeId))
        {
            query = query.Where(b => b.SocieteId == societeId);
        }
        if (!string.IsNullOrEmpty(filter.FilterByClientCommercialId) && int.TryParse(filter.FilterByClientCommercialId, out int clientCommercialId))
        {
            query = query.Where(b => b.ClientEurope.CommercialId == clientCommercialId);
        }
        if (!string.IsNullOrEmpty(filter.FilterClientName))
        {
            query = query.Where(b => b.ClientEurope.Nom.Contains(filter.FilterClientName));
        }
        if (!string.IsNullOrEmpty(filter.SortBy) && !string.IsNullOrEmpty(filter.OrderBy))
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