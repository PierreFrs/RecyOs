// Created by : Pierre FRAISSE
// RecyOs => RecyOs => CommercialQueryExtension.cs
// Created : 2024/03/28 - 11:51
// Updated : 2024/03/28 - 11:51

using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Filters.Extensions;

public static class CommercialQueryExtension
{
    public static IQueryable<Commercial> ApplyFilter<TCommercial>(
        this IQueryable<TCommercial> query, CommercialFilter filter)
        where TCommercial : Commercial, new()
    {
        var mapDictionary = new Dictionary<string, string>
        {
            {"firstname", "Firstname"},
            {"lastname", "Lastname"}
        };
        
        if (!string.IsNullOrEmpty(filter.FilteredByPrenom))
        {
            query = query.Where(x => x.Firstname.Contains(filter.FilteredByPrenom));
        }
        if (!string.IsNullOrEmpty(filter.FilteredByNom))
        {
            query = query.Where(x => x.Lastname.Contains(filter.FilteredByNom));
        }
        else if (!string.IsNullOrWhiteSpace(filter.SortBy) && mapDictionary.ContainsKey(filter.SortBy))
        {
            string mappedSortBy = mapDictionary[filter.SortBy];
            bool isDescending = filter.OrderBy == "desc";
            query = query.OrderByDynamic(mappedSortBy, isDescending);
        }
        else
        {
            query = query.OrderByDescending(obj => obj.Lastname);
        }

        return query;
    }
}