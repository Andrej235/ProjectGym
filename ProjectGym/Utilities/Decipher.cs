using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ProjectGym.Utilities
{
    public class Decipher<T>(T context) where T : DbContext
    {
        public string DecipherDatabase()
        {
            IEnumerable<PropertyInfo> tableProperies = context.GetType().GetProperties().Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));
            var res = string.Join("\n", tableProperies.Select(prop =>
            {
                object? table = prop.GetValue(context);

                if (table is IQueryable<object> queryable)
                    return $"{prop.Name}\n{DecipherTable(queryable)}";

                throw new Exception($"Invalid table name: {prop.Name}");
            }));
            return res;
        }

        private static string DecipherTable(IQueryable<object> table) => string.Join("\n", table.Select(entity => DecipherEntity(entity)));

        private static string DecipherEntity(object entity)
        {
            var properties = entity.GetType().GetProperties().Where(x => !x.PropertyType.IsGenericType);
            return string.Join("\t", properties.Select(prop => $"{prop.Name}: {prop.GetValue(entity)}"));
        }
    }
}
