using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ProjectGym.Utilities
{
    public static class DatabaseSerializationService
    {
        public static string Serialize<TContext>(TContext context) where TContext : DbContext
        {
            var tablesProperty = context
                .GetType()
                .GetProperties()
                .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            string resultingJSON = "{\n";
            resultingJSON += string.Join(", \n", tablesProperty.Select(property =>
            {
                if (property.GetValue(context) is IEnumerable<object> entities)
                {
                    string entitiesJSON = string.Join(", \n", entities.Select(Serialize));
                    string table = $"\"{property.Name}\": [\n{entitiesJSON}\n]";
                    return table;
                }

                return "";
            }));
            resultingJSON += "\n}";

            return resultingJSON;
        }

        private static string Serialize(object entity)
        {
            var properties = entity.GetType().GetProperties().Where(x => !x.PropertyType.IsGenericType);
            var resultingJSON = "{\n"
                + string.Join(", \n", properties.Select(property =>
                {
                    string value = "";
                    if (property.PropertyType == typeof(string))
                        value = $"\"{property.GetValue(entity)}\"";
                    else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(float) || property.PropertyType == typeof(bool))
                        value = $"{property.GetValue(entity)}".ToLower();
                    else if (property.PropertyType == typeof(Guid) || property.PropertyType == typeof(DateOnly) || property.PropertyType == typeof(DateTime))
                        value = $"\"{property.GetValue(entity)}\"";
                    else if (property.PropertyType.IsArray && property.PropertyType.GetElementType() == typeof(byte))
                    {
                        if (property.GetValue(entity) is IEnumerable<byte> bytes)
                            value = $"\"{Convert.ToBase64String([.. bytes])}\"";
                    }
                    else
                        value = "\"Not implemented\"";

                    return $"\"{property.Name}\": {value}";
                }))
                + "\n}";
            return resultingJSON;
        }

    }
}
