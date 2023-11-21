using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class WorkoutReadService(ExerciseContext context, IReadService<Client> clientReadService) : AbstractReadService<Workout, Guid>
    {
        protected override Func<Workout, Guid> PrimaryKey => x => x.Id;

        protected override IQueryable<Workout> GetIncluded(IEnumerable<string>? include)
        {
            var entitiesIncluded = context.Workouts.AsQueryable();

            if (include is null || !include.Any() || include.Contains("none"))
                return entitiesIncluded;

            if (include.Contains("all") || include.Contains("sets") || include.Contains("workoutsets"))
                return entitiesIncluded.Include(x => x.WorkoutSets);

            return entitiesIncluded;
        }

        protected override Expression<Func<Workout, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            return key switch
            {
                "personal" => x => true,
                "client" => x => true,
                "clientid" => x => true,
                _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
            };
        }

        public override async Task<List<Workout>> Get(string? query, int? offset = 0, int? limit = -1, string? include = "all")
        {
            var entitiesQueryable = GetIncluded(SplitIncludeString(include));
            if (query is null)
                return ApplyOffsetAndLimit(entitiesQueryable, offset, limit).Where(x => x.IsPublic).ToList();

            var keyValuePairsInSearchQuery = SplitQueryString(query);
            List<string>? strictKeyValuePair = keyValuePairsInSearchQuery.FirstOrDefault(kvp => kvp[0] == "strict");
            bool isStrictModeEnabled = false;

            if (strictKeyValuePair != null)
            {
                isStrictModeEnabled = strictKeyValuePair[1] == "true";
                keyValuePairsInSearchQuery.Remove(strictKeyValuePair);
            }

            List<Workout> entities = [];
            if (isStrictModeEnabled)
            {
                foreach (var criteria in DecipherQuery(keyValuePairsInSearchQuery))
                    entitiesQueryable = entitiesQueryable.Where(criteria);

                entities = ApplyOffsetAndLimit(entitiesQueryable, offset, limit);
            }
            else
            {
                entities = ApplyOffsetAndLimit(ApplyNonStrictCriterias(entitiesQueryable, DecipherQuery(keyValuePairsInSearchQuery)), offset, limit);
            }



            List<string>? clientKeyValuePair = keyValuePairsInSearchQuery.FirstOrDefault(kvp => kvp[0] == "client" || kvp[0] == "clientid");
            if (clientKeyValuePair != null && Guid.TryParse(clientKeyValuePair[1], out var clientId))
            {
                Guid? creatorId = (await clientReadService.Get(x => x.Id == clientId, "none")).UserGUID;

                List<string>? personalKeyValuePair = keyValuePairsInSearchQuery.FirstOrDefault(kvp => kvp[0] == "personal");
                if (personalKeyValuePair is null)
                    return entities.Where(x => x.IsPublic && x.CreatorId != creatorId).ToList();
                else
                {
                    return personalKeyValuePair[1].ToLower().Trim() == "true"
                        ? entities.Where(x => x.CreatorId == creatorId).ToList()
                        : entities.Where(x => x.IsPublic && x.CreatorId != creatorId).ToList();
                }
            }
            return entities.Where(x => x.IsPublic).ToList();
        }
    }
}
