using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectGym.Data;
using ProjectGym.Utilities;
using System.Linq.Expressions;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProjectGym.Services.Read
{
    public abstract class ReadService<T>(ExerciseContext context) : AbstractReadService<T> where T : class
    {
        protected override IQueryable<T> GetIncluded(IEnumerable<string>? include)
        {
            IQueryable<T> entitiesIncluding = context.Set<T>().AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return entitiesIncluding;

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            var navigationProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (include.Contains("all"))
            {
                foreach (var navigationProperty in navigationProperties)
                    entitiesIncluding = Include(entitiesIncluding, navigationProperty.Name);

                return entitiesIncluding;
            }

            IEnumerable<PropertyInfo> c = navigationProperties.Where(x => include.Any(y => x.Name.ToLower().Contains(y)));
            foreach (var navigationProperty in c)
                entitiesIncluding = Include(entitiesIncluding, navigationProperty.Name);

            return entitiesIncluding;
        }

        private static IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> query, string propertyName) where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<TEntity, object>>(property, parameter);

            return query.Include(lambda);
        }

       /* protected override Expression<Func<T, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            try
            {
                var properties = typeof(T).GetProperties();
                if (key.Contains("id"))
                {
                    key = key.Replace("id", "");
                    if (int.TryParse(value, out int id))
                    {
                        var keyProperty = properties.SingleOrDefault(x => x.Name.ToLower().Contains(key)) ?? throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");

                        if (keyProperty.PropertyType.IsGenericType && keyProperty.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        {
                            var innerPropertyType = keyProperty.PropertyType.GetGenericArguments()[0]; //Muscle

                            var parameter = Expression.Parameter(innerPropertyType, "x"); //Muscle x
                            var propertyAccess = Expression.Property(parameter, "Id"); //Muscle x.Id

                            var enumerable = Expression.Convert(keyProperty, typeof(IEnumerable<object>));

                            // Call Any method on the IEnumerable<MyObject>
                            MethodInfo anyMethod = typeof(Enumerable).GetMethods()
                                .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
                                .MakeGenericMethod(typeof(T));

                        }



                        //return Expression.Lambda<Func<T, bool>>(comparison, parameter);
                    }
                    else if (Guid.TryParse(value, out Guid guid))
                    {
                        throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
                    }
                }
                throw new NotSupportedException($"Invalid search query value. Entered value: {value}");
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                throw;
            }
        }*/
    }
}
