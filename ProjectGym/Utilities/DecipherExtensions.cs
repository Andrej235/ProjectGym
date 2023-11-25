using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ProjectGym.Utilities
{
    public static class DecipherExtensions
    {
        public static string Decipher<T>(this T context) where T : DbContext
        {
            IEnumerable<PropertyInfo> tableProperies = context.GetType().GetProperties().Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));
            return string.Join("\n\n", tableProperies.Select(prop =>
            {
                object? table = prop.GetValue(context);

                if (table is IQueryable<object> queryable)
                    return $"{prop.Name}\n{queryable.Decipher()}";

                throw new Exception($"Invalid table name: {prop.Name}");
            }));
        }

        public static string Decipher(this IQueryable<object> table) => string.Join("\n", table.Select(entity => entity.Decipher()));

        public static string Decipher(this object entity)
        {
            var properties = entity.GetType().GetProperties().Where(x => !x.PropertyType.IsGenericType);
            return string.Join("\t", properties.Select(prop => $"{prop.Name}: {prop.GetValue(entity)}"));
        }
    }
}
