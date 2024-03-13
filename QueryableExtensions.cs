using System;
using System.Linq;
using System.Linq.Expressions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> OrderByProperty<TEntity>(this IQueryable<TEntity> source, string propertyName, bool ascending)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return source;
        }

        var type = typeof(TEntity);
        var property = type.GetProperty(propertyName);

        if (property == null)
        {
            return source;
        }

        var parameter = Expression.Parameter(type, "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExp = Expression.Lambda(propertyAccess, parameter);

        var methodName = ascending ? "OrderBy" : "OrderByDescending";
        var resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));

        return source.Provider.CreateQuery<TEntity>(resultExp);
    }
}
