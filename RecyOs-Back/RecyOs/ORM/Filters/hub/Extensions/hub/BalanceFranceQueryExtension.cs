// BalanceQueryExtension.cs -
// ======================================================================0
// Crée par : brollin
// Fichier Crée le : 07/02/2025
// Fichier Modifié le : 07/02/2025
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Filters.Extensions.hub;

public static class BalanceFranceQueryExtension
{
    public static IQueryable<BalanceFrance> ApplyFilter<TBalanceFrance>(this IQueryable<TBalanceFrance> query, BalanceFranceGridFilter filter)
    where TBalanceFrance : BalanceFrance, new()
    {
        var mapDictionary = new Dictionary<string, string>
        {
            {"clientId", "ClientId"},
            {"societeId", "SocieteId"},
            {"clientCommercialId", "EtablissementClient.CommercialId"},
            {"montant", "Montant"},
            {"dateRecuperationBalance", "DateRecuperationBalance"},
            {"etablissementClient.nom", "EtablissementClient.Nom"},
            {"societe.nom", "Societe.Nom"},
            {"commercial", "EtablissementClient.Commercial.Lastname"},
            {"encoursMax", "EtablissementClient.EncoursMax"},
            
        };
        
        if (filter == null)
        {
            return query;
        }
        if (!string.IsNullOrWhiteSpace(filter.FilterByClientId) && int.TryParse(filter.FilterByClientId, out int clientId))
        {
            query = query.Where(b => b.ClientId == clientId);
        }
        if (!string.IsNullOrWhiteSpace(filter.FilterBySocieteId) && int.TryParse(filter.FilterBySocieteId, out int societeId))
        {
            query = query.Where(b => b.SocieteId == societeId);
        }
        if (!string.IsNullOrWhiteSpace(filter.FilterByClientCommercialId) && int.TryParse(filter.FilterByClientCommercialId, out int clientCommercialId))
        {
            query = query.Where(b => b.EtablissementClient.CommercialId.HasValue && b.EtablissementClient.CommercialId.Value == clientCommercialId);
        }
        if (!string.IsNullOrWhiteSpace(filter.FilterClientName))
        {
            query = query.Where(b => b.EtablissementClient.Nom.Contains(filter.FilterClientName));
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

    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string sortColumn, bool descending)
    {
        // Création du paramètre de la lambda (x => ...)
        var parameter = Expression.Parameter(typeof(T), "x");
        Expression propertyAccess = parameter;

        // Gérer les propriétés imbriquées en découpant la chaîne
        foreach (var member in sortColumn.Split('.'))
        {
            propertyAccess = Expression.Property(propertyAccess, member);
        }
        
        // Construire le type de la lambda : Func<T, TKey>
        var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), propertyAccess.Type);
        var lambda = Expression.Lambda(delegateType, propertyAccess, parameter);

        // Déterminer la méthode à appeler (OrderBy ou OrderByDescending)
        string methodName = descending ? "OrderByDescending" : "OrderBy";

        // Construire l'expression d'appel en encadrant la lambda avec Expression.Quote
        var resultExp = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { typeof(T), propertyAccess.Type },
            query.Expression,
            Expression.Quote(lambda));

        return query.Provider.CreateQuery<T>(resultExp);
    }
}
