using Microsoft.EntityFrameworkCore;
using ProjectGym.Utilities;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public abstract class AbstractReadService<T> : IReadService<T> where T : class
    {
        public virtual async Task<T> Get(Expression<Func<T, bool>> criteria, string? include = "all")
        {
            IQueryable<T> entitesQueryable = GetIncluded(SplitIncludeString(include));
            T? entity = await entitesQueryable.FirstOrDefaultAsync(criteria);
            return entity ?? throw new NullReferenceException();
        }

        public async Task<T> Get(object id, string? include = "all")
        {
            return await Task.Run(() =>
            {
                IQueryable<T> entitesQueryable = GetIncluded(SplitIncludeString(include));
                bool CheckPrimaryKey(T e)
                {
                    var idProp = e.GetType().GetProperty("Id");
                    if (idProp == null)
                        return false;

                    return Convert.ToString(idProp.GetValue(e)) == Convert.ToString(id);
                }

                T? entity = entitesQueryable.AsEnumerable().FirstOrDefault(CheckPrimaryKey);
                return entity ?? throw new NullReferenceException();
            });
        }

        public virtual async Task<List<T>> Get(Expression<Func<T, bool>> criteria, int? offset = 0, int? limit = -1, string? include = "all")
        {
            return await Task.Run(() =>
            {
                IQueryable<T> entitesQueryable = GetIncluded(SplitIncludeString(include));
                return ApplyOffsetAndLimit(entitesQueryable.Where(criteria), offset, limit);
            });
        }

        public virtual async Task<List<T>> Get(string? query, int? offset = 0, int? limit = -1, string? include = "all")
        {
            return await Task.Run(() =>
            {
                var entitiesQueryable = GetIncluded(SplitIncludeString(include));
                if (query is null)
                    return ApplyOffsetAndLimit(entitiesQueryable, offset, limit);

                var keyValuePairsInSearchQuery = SplitQueryString(query);
                List<string>? strictKeyValuePair = keyValuePairsInSearchQuery.FirstOrDefault(kvp => kvp[0] == "strict");
                bool isStrictModeEnabled = false;

                if (strictKeyValuePair != null)
                {
                    isStrictModeEnabled = strictKeyValuePair[1] == "true";
                    keyValuePairsInSearchQuery.Remove(strictKeyValuePair);
                }

                if (isStrictModeEnabled)
                {
                    foreach (var criteria in DecipherQuery(keyValuePairsInSearchQuery))
                        entitiesQueryable = entitiesQueryable.Where(criteria);
                }
                else
                {
                    return ApplyOffsetAndLimit(ApplyNonStrictCriterias(entitiesQueryable, DecipherQuery(keyValuePairsInSearchQuery)), offset, limit);
                }

                return ApplyOffsetAndLimit(entitiesQueryable, offset, limit);
            });
        }

        protected abstract IQueryable<T> GetIncluded(IEnumerable<string>? include);
        protected abstract Expression<Func<T, bool>> TranslateKeyValueToExpression(string key, string value);

        protected static IEnumerable<string>? SplitIncludeString(string? include) => include?.ToLower().Replace(" ", "").Split(',').Where(x => !string.IsNullOrWhiteSpace(x));
        //TODO?: make ~=(=) mean one of x,y,z and =(==) mean x and y and z
        protected static List<List<string>> SplitQueryString(string query) => query.Split(';')
            .Select(sq => sq.Split('=').Select(x => x.Trim().ToLower()).ToList())
            .Where(x => x.Count == 2)
            .ToList();

        protected IEnumerable<Expression<Func<T, bool>>> DecipherQuery(string query)
        {
            var keyValuePairsInSearchQuery = SplitQueryString(query);
            foreach (var keyValue in keyValuePairsInSearchQuery)
            {
                Expression<Func<T, bool>>? current = null;
                try
                {
                    current = TranslateKeyValueToExpression(keyValue[0], keyValue[1]);
                }
                catch (Exception ex)
                {
                    LogDebugger.LogError(ex);
                }

                if (current == null)
                    continue;
                yield return current;
            }
        }
        protected IEnumerable<Expression<Func<T, bool>>> DecipherQuery(List<List<string>> keyValuePairsInSearchQuery)
        {
            foreach (var keyValue in keyValuePairsInSearchQuery)
            {
                Expression<Func<T, bool>>? current = null;
                try
                {
                    current = TranslateKeyValueToExpression(keyValue[0], keyValue[1]);
                }
                catch (Exception ex)
                {
                    LogDebugger.LogError(ex);
                }

                if (current == null)
                    continue;
                yield return current;
            }
        }

        protected virtual IQueryable<T> ApplyNonStrictCriterias(IQueryable<T> entitiesQueryable, IEnumerable<Expression<Func<T, bool>>> criterias)
        {
            criterias = criterias
            .Where(x => x.Body is not ConstantExpression);

            if(!criterias.Any())
                return entitiesQueryable;

            return criterias
            .Select(x => entitiesQueryable.Where(x))
            .SelectMany(x => x)
            .GroupBy(x => x)
            .OrderByDescending(g => g.Count())
            .Select(x => x.First())
            .AsQueryable();
        }

        protected List<T> ApplyOffsetAndLimit(IQueryable<T> queryable, int? offset = 0, int? limit = -1)
        {
            queryable = queryable.Skip(offset ?? 0);

            if (limit != null && limit >= 0)
                queryable = queryable.Take(limit ?? 0);

            return [.. queryable];
        }

        //**************************************************************
        //Less efficient than just doing it in function
        //protected static bool IsStrictModeEnabledInQuery(string query) => SplitQueryString(query).FirstOrDefault(kvp => kvp[0] == "strict")?[1] == "true";
        //protected static bool IsStrictModeEnabledInQuery(List<List<string>> keyValuePairsInSearchQuery) => keyValuePairsInSearchQuery.FirstOrDefault(kvp => kvp[0] == "strict")?[1] == "true";
        //**************************************************************
    }
}
