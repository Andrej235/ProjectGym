using Microsoft.EntityFrameworkCore;
using ProjectGym.Exceptions;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace ProjectGym.Utilities
{
    public static class DatabaseDeserializationService
    {
        struct OldNewIdPairs(object oldId, object newId)
        {
            public object oldId = oldId;
            public object newId = newId;
        }

        /// <summary>
        /// Loads the given data into the given database (context)
        /// </summary>
        /// <typeparam name="TContext">Type of a database context in which the data will be injected</typeparam>
        /// <param name="context">Instance of TContext which will be used to inject data into the database</param>
        /// <param name="jsonEncodedDataBase">Data in a json format, will be translated into objects and injected into the database</param>
        /// <param name="tableAliases">
        /// Each table can have multiple aliases
        /// <br/>Each alias is a KeyValuePair<string, string> where
        ///     <br/>Key - Alias
        ///     <br/>Value - Name of the aliased table
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">asdsad</exception>
        public static async Task LoadDatabaseAsync<TContext>(TContext context, string jsonEncodedDataBase, params KeyValuePair<string, string>[] tableAliases) where TContext : DbContext
        {
            IEnumerable<PropertyInfo> tablesAsProperties = GetTablesAsProperties(context);
            Dictionary<string, object> jsonBackupKeyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonEncodedDataBase) ?? throw new ArgumentNullException(nameof(jsonEncodedDataBase));
            Dictionary<string, IEnumerable<OldNewIdPairs>> idPairs = [];

            foreach (PropertyInfo tableProperty in tablesAsProperties)
            {
                Type entityType = tableProperty.PropertyType.GetGenericArguments()[0];
                if (jsonBackupKeyValuePairs[tableProperty.Name] is not JsonElement tableAsJsonElement)
                    continue;

                var currentTableIdPairs = await LoadTableAsync(context, ProcessJSON(tableAsJsonElement), entityType, idPairs, tableAliases);
                idPairs.Add(tableProperty.Name, currentTableIdPairs);
            }
        }

        private static async Task<IEnumerable<OldNewIdPairs>> LoadTableAsync<TContext>(TContext context, dynamic table, Type entityType, IDictionary<string, IEnumerable<OldNewIdPairs>> idPairs, KeyValuePair<string, string>[] aliases) where TContext : DbContext
        {
            List<OldNewIdPairs> tableIdPairs = [];
            foreach (var entity in table)
            {
                object newEntityInstance = Activator.CreateInstance(entityType) ?? throw new NullReferenceException();
                object? oldId = null;
                foreach (var property in (IDictionary<string, object>)entity)
                {
                    string propertyName = property.Key;
                    object propertyValue = property.Value;
                    PropertyInfo? entityPropertyInfo = entityType.GetProperty(propertyName);
                    if (entityPropertyInfo == null)
                        continue;

                    if (Convert.ToString(propertyValue) == "Not implemented")
                        continue;

                    if (propertyName == "Id")
                    {
                        oldId = propertyValue;
                        continue;
                    }

                    if (propertyName.Contains("Id"))
                    {
                        string specificTableName = propertyName.Replace("Id", "");
                        if (!idPairs.TryGetValue(specificTableName + "s", out IEnumerable<OldNewIdPairs>? idPairsOfSpecificTable) && !idPairs.TryGetValue(specificTableName, out idPairsOfSpecificTable))
                        {
                            var tableAliases = aliases.Where(x => x.Key == specificTableName || x.Key == specificTableName + "s").ToList();
                            if (tableAliases.Count == 0)
                                throw new PropertyNotFoundException();

                            if (!idPairs.TryGetValue(tableAliases.First().Value + "s", out idPairsOfSpecificTable) && !idPairs.TryGetValue(tableAliases.First().Value, out idPairsOfSpecificTable))
                                throw new PropertyNotFoundException();
                        }

                        OldNewIdPairs specificIdPair = idPairsOfSpecificTable.First(x => Convert.ToString(x.oldId) == Convert.ToString(propertyValue));
                        entityPropertyInfo.SetValue(newEntityInstance, specificIdPair.newId);
                        continue;
                    }

                    if (entityPropertyInfo.PropertyType.IsArray && entityPropertyInfo.PropertyType.GetElementType() == typeof(byte))
                    {
                        entityPropertyInfo.SetValue(newEntityInstance, Convert.FromBase64String(Convert.ToString(propertyValue) ?? throw new Exception($"Property: {property.Key} is entered as an empty string. (byte[])")));
                        continue;
                    }

                    if (entityPropertyInfo.PropertyType == typeof(DateTime))
                    {
                        if (DateTime.TryParse(Convert.ToString(propertyValue), out DateTime date))
                            entityPropertyInfo.SetValue(newEntityInstance, date);
                        continue;
                    }

                    if (entityPropertyInfo.PropertyType == typeof(DateTime))
                    {
                        if (DateOnly.TryParse(Convert.ToString(propertyValue), out DateOnly date))
                            entityPropertyInfo.SetValue(newEntityInstance, date);
                        continue;
                    }

                    entityPropertyInfo.SetValue(newEntityInstance, propertyValue);
                }

                if (oldId == null)
                    throw new PropertyNotFoundException("Id");

                await context.AddAsync(newEntityInstance);
                await context.SaveChangesAsync();

                object newId = newEntityInstance.GetType().GetProperty("Id")?.GetValue(newEntityInstance) ?? throw new PropertyNotFoundException("Id");
                tableIdPairs.Add(new OldNewIdPairs(oldId, newId));
            }
            return tableIdPairs;
        }

        private static IEnumerable<PropertyInfo> GetTablesAsProperties<TContext>(TContext context) where TContext : DbContext => context
            .GetType()
            .GetProperties()
            .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .OrderBy(x => x.PropertyType.GetGenericArguments()[0].GetProperties().Select(x => x.Name).Where(x => x.Contains("Id")).Count());

        private static object ProcessJSON(JsonElement jsonElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.Undefined:
                    throw new NullReferenceException();
                case JsonValueKind.Object:
                    dynamic entity = new ExpandoObject();
                    foreach (JsonProperty property in jsonElement.EnumerateObject())
                        ((IDictionary<string, object>)entity).Add(property.Name, ProcessJSON(property.Value)); //Replace property.Value with ProcessJSONProperty(property.Value) for fully deciphered objects

                    return entity;
                case JsonValueKind.Array:
                    List<dynamic> expandoList = [];
                    expandoList.AddRange(jsonElement.EnumerateArray().Select(ProcessJSON));

                    return expandoList;
                case JsonValueKind.String:
                    return jsonElement.GetString() ?? "";
                case JsonValueKind.Number:
                    return jsonElement.GetInt32();
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Null:
                    throw new NullReferenceException();
                default:
                    break;
            }

            return jsonElement.ValueKind switch
            {
                JsonValueKind.Number => jsonElement.GetInt32(),
                JsonValueKind.String => jsonElement.GetString() ?? "",
                JsonValueKind.False or JsonValueKind.True => jsonElement.GetBoolean(),
                _ => throw new NotImplementedException()
            };
        }
    }
}



/*        static bool IsNullable(Type type)
        {
            if (!type.IsValueType) return true; // Reference type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable value type
            return false; // Non-nullable value type
        }

        static void CheckAllNonNullableProperties(object entity)
        {
            foreach (var property in entity.GetType().GetProperties().Where(x => IsNullable(x.PropertyType)))
            {
                if (property.GetValue(entity) == null)
                {
                    if (property.PropertyType.IsValueType)
                    {
                        property.SetValue(entity, Activator.CreateInstance(property.PropertyType));
                    }
                    else
                    {
                        var defaultConstructor = property.PropertyType.GetConstructor(Type.EmptyTypes);
                        if (defaultConstructor != null)
                        {
                            property.SetValue(entity, Activator.CreateInstance(property.PropertyType));
                        }
                        else
                        {
                            throw new InvalidOperationException($"Cannot create an instance of type {property.PropertyType.Name}. Consider providing a default constructor or handle this case differently.");
                        }
                    }
                }
            }
        }*/