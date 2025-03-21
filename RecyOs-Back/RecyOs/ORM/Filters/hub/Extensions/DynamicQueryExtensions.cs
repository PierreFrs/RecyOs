// /** DynamicQueryExtensions.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 22/01/2021
//  * Fichier Modifié le : 29/06/2021
//  * Code développé pour le projet : Archimede.Common
//  */

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RecyOs.ORM.Filters.Extensions;

public static class DynamicQueryExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string sortColumn, bool descending)
{
    var parameter = Expression.Parameter(typeof(T), "p");
    var orderCommand = descending ? "OrderByDescending" : "OrderBy";

    PropertyInfo property = GetNestedProperty(typeof(T), sortColumn);
    var propertyAccess = BuildPropertyAccess(parameter, property, sortColumn);

    if (property == null) return query;

    var orderByExpression = Expression.Lambda(propertyAccess, parameter);
    Expression resultExpression = Expression.Call(typeof(Queryable), orderCommand,
        new[] { typeof(T), property.PropertyType },
        query.Expression, Expression.Quote(orderByExpression));

    return query.Provider.CreateQuery<T>(resultExpression);
}

public static PropertyInfo GetNestedProperty(Type type, string propName)
{
    PropertyInfo property;
    if (propName.Contains('.'))
    {
        var childProperties = propName.Split('.');
        property = type.GetProperty(childProperties[0], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
    }
    else
    {
        property = type.GetProperty(propName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
    }

    return property;
}

public static Expression BuildPropertyAccess(ParameterExpression parameter, PropertyInfo property, string propName)
{
    Expression propertyAccess = null;
    if (property != null)
    {
        propertyAccess = Expression.MakeMemberAccess(parameter, property);
        if (propName.Contains('.'))
        {
            var childProperties = propName.Split('.');
            for (var i = 1; i < childProperties.Length; i++)
            {
                property = property.PropertyType.GetProperty(childProperties[i], BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (property != null)
                {
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
        }
    }
    return propertyAccess;
}

    }