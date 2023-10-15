using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectGym.Controllers;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Diagnostics;
using System.Linq.Expressions;

namespace ProjectGym.Services
{
    public class ExerciseService
    {
        private readonly ExerciseContext exerciseContext;
        public ExerciseService(ExerciseContext exerciseContext)
        {
            this.exerciseContext = exerciseContext;
        }



        public async Task<List<Exercise>> Get(IQueryable<Exercise> exercisesQueryable, string? searchQuery, int? offset = 0, int? limit = -1)
        {
            if (searchQuery is null)
                return await Get(exercisesQueryable, offset, limit);

            var criterias = new List<Expression<Func<Exercise, bool>>>();
            var keyValuePairsInSearchQuery = searchQuery.Split(';')
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
                    exercisesQueryable = exercisesQueryable.Where(criteria);
            }
            else
            {
                var exercises = criterias.Select(x => exercisesQueryable.Where(x))
                    .SelectMany(q => q)
                    .GroupBy(e => e.Id)
                    .OrderByDescending(g => g.Count())
                    .SelectMany(g => g)
                    .DistinctBy(e => e.Id)
                    .Skip(offset ?? 0);

                if (limit is not null && limit >= 0)
                    exercises = exercises.Take(limit ?? 0);

                return exercises.ToList();
            }

            exercisesQueryable = exercisesQueryable.Skip(offset ?? 0);

            if (limit is not null && limit >= 0)
                exercisesQueryable = exercisesQueryable.Take(limit ?? 0);

            return await exercisesQueryable.ToListAsync();
        }
        private static Expression<Func<Exercise, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                /*                var ids = exercisesQueryable.AsEnumerable().Where(e => e.Name.IsSimilar(value)).Select(e => e.Id);
                                return e => ids.Contains(e.Id);*/

                return e => e.Name.Contains(value, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                if (int.TryParse(value, out int valueId))
                {
                    return key switch
                    {
                        "id" => e => e.Id == valueId,
                        "category" => e => e.CategoryId == valueId,
                        "primarymuscle" => e => e.PrimaryMuscles.Any(m => m.Id == valueId),
                        "secondarymuscle" => e => e.SecondaryMuscles.Any(m => m.Id == valueId),
                        "equipment" => e => e.Equipment.Any(eq => eq.Id == valueId),
                        _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                    };
                }
                else
                {
                    if (value.Contains(','))
                    {
                        var values = value.Replace(" ", "").Split(',');
                        List<int> valueIds = new();
                        foreach (var id in values)
                        {
                            if (int.TryParse(id, out int newId))
                                valueIds.Add(newId);
                        }

                        return key switch
                        {
                            "id" => e => valueIds.Contains(e.Id),
                            "category" => e => valueIds.Contains(e.CategoryId),
                            "primarymuscle" => e => e.PrimaryMuscles.Any(m => valueIds.Contains(m.Id)),
                            "secondarymuscle" => e => e.SecondaryMuscles.Any(m => valueIds.Contains(m.Id)),
                            "equipment" => e => e.Equipment.Any(eq => valueIds.Contains(eq.Id)),
                            _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                        };
                    }

                    throw new NotSupportedException($"Invalid search query value. Entered value: {value}");
                }
            }
        }



        public async Task<List<Exercise>> Get(IQueryable<Exercise> exercisesQueryable, int? offset = 0, int? limit = -1)
        {
            exercisesQueryable = exercisesQueryable.Skip(offset ?? 0);

            if (limit != null && limit >= 0)
                exercisesQueryable = exercisesQueryable.Take(limit ?? 0);

            return await exercisesQueryable.ToListAsync();
        }

        public async Task<Exercise?> Get(Expression<Func<Exercise, bool>> criteria, IQueryable<Exercise> exercisesQueryable) => await exercisesQueryable.FirstOrDefaultAsync(criteria);



        public ExerciseDTO TranslateToDTO(Exercise? exercise) => exercise == null ? new() : new()
        {
            Id = exercise.Id,
            Name = exercise.Name,
            Description = exercise.Description,
            Images = exercise.Images.Select(i => new ImageDTO()
            {
                Id = i.Id,
                ImageURL = i.ImageURL,
                IsMain = i.IsMain,
                Style = i.Style
            }),
            AliasIds = exercise.Aliases.Select(a => a.Id),
            CategoryId = exercise.CategoryId,
            EquipmentIds = exercise.Equipment.Select(a => a.Id),
            IsVariationOfIds = exercise.IsVariationOf.Select(a => a.Id),
            VariationIds = exercise.VariationExercises.Select(a => a.Id),
            NoteIds = exercise.Notes.Select(a => a.Id),
            PrimaryMuscleIds = exercise.PrimaryMuscles.Select(a => a.Id),
            SecondaryMuscleIds = exercise.SecondaryMuscles.Select(a => a.Id),
            VideoIds = exercise.Videos.Select(a => a.Id),
        };
        public List<ExerciseDTO> TranslateToDTO(List<Exercise> exercises) => exercises.Select(TranslateToDTO).ToList();
        public AdvancedDTO<T> TranslateToAdvancedDTO<T>(List<T> values, string baseAPIUrl, int offset, int limit)
        {
            AdvancedDTO<T> res = new()
            {
                BatchSize = values.Count,
                PreviousBatchURLExtension = null,
                NextBatchURLExtension = null,
                Values = values,
            };



            if (limit >= 0 && values.Count >= limit)
            {
                var nextOffset = offset + limit;
                res.NextBatchURLExtension = $"{baseAPIUrl}&offset={nextOffset}&limit={limit}";
            }

            if (offset > 0)
            {
                var previousOffset = offset - limit;
                if (previousOffset < 0)
                    previousOffset = 0;

                res.PreviousBatchURLExtension = $"{baseAPIUrl}&offset={previousOffset}&limit={limit}";
            }

            return res;
        }



        public IQueryable<Exercise> GetIncluded(string? include)
        {
            IQueryable<Exercise> exercisesIncluding = exerciseContext.Exercises.AsQueryable();
            if (include == null)
                return exercisesIncluding;

            string[] toInclude = include.ToLower().Replace(" ", "").Split(',');
            if (toInclude.Contains("all"))
            {
                return GetIncluded();
            }
            else if (toInclude.Contains("none"))
            {
                return exercisesIncluding;
            }
            else
            {
                foreach (var inc in toInclude)
                {
                    exercisesIncluding = inc switch
                    {
                        "images" => exercisesIncluding.Include(e => e.Images),
                        "videos" => exercisesIncluding.Include(e => e.Videos),
                        "variations" => exercisesIncluding.Include(e => e.VariationExercises),
                        "variationof" => exercisesIncluding.Include(e => e.IsVariationOf),
                        "category" => exercisesIncluding.Include(e => e.Category),
                        "primarymuscles" => exercisesIncluding.Include(e => e.PrimaryMuscles),
                        "secondarymuscles" => exercisesIncluding.Include(e => e.SecondaryMuscles),
                        "equipment" => exercisesIncluding.Include(e => e.Equipment), //Everything works except equipment, it's not properly connected to exercises
                        "aliases" => exercisesIncluding.Include(e => e.Aliases),
                        "notes" => exercisesIncluding.Include(e => e.Notes),
                        _ => exercisesIncluding
                    };
                }
            }
            return exercisesIncluding;
        }
        public IQueryable<Exercise> GetIncluded() => exerciseContext.Exercises
            .Include(e => e.VariationExercises)
            .Include(e => e.IsVariationOf)
            .Include(e => e.Category)
            .Include(e => e.PrimaryMuscles)
            .Include(e => e.SecondaryMuscles)
            .Include(e => e.Equipment)
            .Include(e => e.Videos)
            .Include(e => e.Aliases)
            .Include(e => e.Notes)
            .Include(e => e.Images);
    }

    public class ExerciseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public int CategoryId { get; set; }
        public IEnumerable<ImageDTO> Images { get; set; } = null!;
        public IEnumerable<int> VideoIds { get; set; } = null!;
        public IEnumerable<int> IsVariationOfIds { get; set; } = null!;
        public IEnumerable<int> VariationIds { get; set; } = null!;
        public IEnumerable<int> EquipmentIds { get; set; } = null!;
        public IEnumerable<int> PrimaryMuscleIds { get; set; } = null!;
        public IEnumerable<int> SecondaryMuscleIds { get; set; } = null!;
        public IEnumerable<int> AliasIds { get; set; } = null!;
        public IEnumerable<int> NoteIds { get; set; } = null!;
    }

    public class ImageDTO
    {
        public int Id { get; set; }
        public string ImageURL { get; set; } = null!;
        public bool IsMain { get; set; }
        public string Style { get; set; } = null!;
    }
}