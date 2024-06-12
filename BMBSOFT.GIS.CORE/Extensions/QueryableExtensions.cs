using System;
using System.Linq;
using System.Linq.Expressions;

namespace BASE.CORE.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> func)
        {
            if (condition)
                return query.Where(func);

            return query;
        }
    }
}
