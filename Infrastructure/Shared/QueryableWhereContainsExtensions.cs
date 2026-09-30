using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared
{
    internal static class QueryableWhereContainsExtensions
    {
        private static readonly MethodInfo ILikeMethod = typeof(NpgsqlDbFunctionsExtensions)
            .GetMethod(nameof(NpgsqlDbFunctionsExtensions.ILike), [typeof(DbFunctions), typeof(string), typeof(string)])!;

        public static IQueryable<TEntity> WhereContains<TEntity>(this IQueryable<TEntity> query, Expression<Func<TEntity, string>> selector, string? value)
        {
            if (string.IsNullOrEmpty(value))
                return query;

            MethodCallExpression iLikeCall = Expression.Call(
                ILikeMethod,
                Expression.Constant(EF.Functions),
                selector.Body,
                Expression.Constant(Contains(value)));

            Expression<Func<TEntity, bool>> predicate = Expression.Lambda<Func<TEntity, bool>>(iLikeCall, selector.Parameters);

            return query.Where(predicate);
        }

        private static string Contains(string value)
        {
            return $"%{EscapeWildcards(value)}%";
        }

        private static string EscapeWildcards(string value)
        {
            return value
                .Replace("\\", "\\\\")
                .Replace("%", "\\%")
                .Replace("_", "\\_");
        }
    }
}