﻿using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using System.Linq.Expressions;
using System.Reflection;

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
    }
}
