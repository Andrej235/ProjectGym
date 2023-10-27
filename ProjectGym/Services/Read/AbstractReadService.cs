using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace ProjectGym.Services.Read
{
    public abstract class AbstractReadService<T, TPrimaryKey> : IReadService<T> where T : class
    {
        protected abstract Func<T, TPrimaryKey> PrimaryKey { get; }

        public virtual async Task<T> Get(Expression<Func<T, bool>> criteria, string? include = "all") => await GetIncluded(SplitIncludeString(include)).FirstOrDefaultAsync(criteria) ?? throw new NullReferenceException("Entity not found");
        public virtual async Task<List<T>> Get(Expression<Func<T, bool>> criteria, int? offset = 0, int? limit = -1, string? include = "all") => await ApplyOffsetAndLimit(GetIncluded(SplitIncludeString(include)).Where(criteria), offset, limit);
        public virtual async Task<List<T>> Get(string? query, int? offset = 0, int? limit = -1, string? include = "all")
        {
            var entitiesQueryable = GetIncluded(SplitIncludeString(include));

            if (query is null)
                return await ApplyOffsetAndLimit(entitiesQueryable, offset, limit);

            var criterias = new List<Expression<Func<T, bool>>>();
            var keyValuePairsInSearchQuery = query.Split(';')
                                                        .Select(sq => sq.Split('=')
                                                        .Select(x => x.Trim().ToLower())
                                                        .ToList())
                                                        .Where(x => x.Count == 2)
                                                        .ToList();

            List<string>? strictKeyValuePair = keyValuePairsInSearchQuery.FirstOrDefault(kvp => kvp[0] == "strict");
            bool isStrictModeEnabled = true;

            if (strictKeyValuePair is not null)
            {
                isStrictModeEnabled = strictKeyValuePair[1] is null || strictKeyValuePair[1] == "true";
                keyValuePairsInSearchQuery.Remove(strictKeyValuePair);
            }

            foreach (var keyValue in keyValuePairsInSearchQuery)
            {
                try
                {
                    criterias.Add(TranslateKeyValueToExpression(keyValue[0], keyValue[1]));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"---> Exception occured: {ex}");
                }
            }

            if (isStrictModeEnabled)
            {
                foreach (var criteria in criterias)
                    entitiesQueryable = entitiesQueryable.Where(criteria);
            }
            else
            {
                var exercises = criterias.Select(x => entitiesQueryable.Where(x))
                    .SelectMany(q => q)
                    .GroupBy(PrimaryKey)
                    .OrderByDescending(g => g.Count())
                    .SelectMany(g => g)
                    .DistinctBy(PrimaryKey)
                    .Skip(offset ?? 0);

                if (limit is not null && limit >= 0)
                    exercises = exercises.Take(limit ?? 0);

                return exercises.ToList();
            }

            return await ApplyOffsetAndLimit(entitiesQueryable, offset, limit);
        }

        protected abstract IQueryable<T> GetIncluded(IEnumerable<string>? include);
        protected abstract Expression<Func<T, bool>> TranslateKeyValueToExpression(string key, string value);
        protected async Task<List<T>> ApplyOffsetAndLimit(IQueryable<T> queryable, int? offset = 0, int? limit = -1)
        {
            queryable = queryable.Skip(offset ?? 0);

            if (limit != null && limit >= 0)
                queryable = queryable.Take(limit ?? 0);

            return await queryable.ToListAsync();
        }
        private static IEnumerable<string>? SplitIncludeString(string? include) => include?.ToLower().Replace(" ", "").Split(',').Where(x => !string.IsNullOrWhiteSpace(x));
    }
}
