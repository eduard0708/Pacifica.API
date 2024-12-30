using System.Linq.Expressions;

namespace Pacifica.API.Helper
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property {propertyName} not found on type {typeof(T)}", nameof(propertyName));
            }

            // Create the parameter expression for the property
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyInfo);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            // Dynamically call OrderBy
            var methodCallExpression = Expression.Call(
                typeof(Queryable),
                "OrderBy",
                new Type[] { typeof(T), typeof(object) },
                source.Expression,
                lambda
            );

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property {propertyName} not found on type {typeof(T)}", nameof(propertyName));
            }

            // Create the parameter expression for the property
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyInfo);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            // Dynamically call OrderByDescending
            var methodCallExpression = Expression.Call(
                typeof(Queryable),
                "OrderByDescending",
                new Type[] { typeof(T), typeof(object) },
                source.Expression,
                lambda
            );

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}


