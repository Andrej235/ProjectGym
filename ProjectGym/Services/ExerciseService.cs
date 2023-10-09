using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Exercise>> Get(IQueryable<Exercise> exercisesQueryable, int? offset = 0, int? limit = -1)
        {
            exercisesQueryable = exercisesQueryable.Skip(offset ?? 0);

            if (limit != null && limit >= 0)
                exercisesQueryable = exercisesQueryable.Take(limit ?? 0);

            return await exercisesQueryable.ToListAsync();
        }

        public async Task<List<Exercise>> Get(IQueryable<Exercise> exercisesQueryable, string? searchQuery, int? offset = 0, int? limit = -1)
        {
            if (searchQuery is null)
                return await Get(exercisesQueryable, offset, limit);

            var keyValuePairsInSearchQuery = searchQuery.Split(';');
            var criterias = new List<Expression<Func<Exercise, bool>>>();
            foreach (var keyValuePair in keyValuePairsInSearchQuery)
            {
                var keyValue = keyValuePair.Split('=');

                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim().ToLower();
                    string value = keyValue[1].Trim().ToLower();

                    switch (key)
                    {
                        case "id":
                            int id;
                            if (int.TryParse(value, out id))
                                criterias.Add(e => e.Id == id);
                            else
                                Debug.WriteLine($"---> Invalid search query value. Entered value: {value}");
                            break;
                        case "category":
                            int categoryId;
                            if (int.TryParse(value, out categoryId))
                                criterias.Add(e => e.CategoryId == categoryId);
                            else
                                Debug.WriteLine($"---> Invalid search query value. Entered value: {value}");
                            break;
                        case "primarymuscle":
                            int primaryMuscleId;
                            if (int.TryParse(value, out primaryMuscleId))
                                criterias.Add(e => e.PrimaryMuscles.Any(m => m.Id == primaryMuscleId));
                            else
                                Debug.WriteLine($"---> Invalid search query value. Entered value: {value}");
                            break;
                        default:
                            Debug.WriteLine($"---> Invalid search query key. Entered key: {key}");
                            break;
                    }
                }
                else
                {
                    Debug.WriteLine($"---> Invalid search query key value pair parameter. Key value pair: {keyValuePair}");
                }
            }

            foreach (var criteria in criterias)
                exercisesQueryable = exercisesQueryable.Where(criteria);

            exercisesQueryable = exercisesQueryable.Skip(offset ?? 0);

            if (limit != null && limit >= 0)
                exercisesQueryable = exercisesQueryable.Take(limit ?? 0);

            return await exercisesQueryable.ToListAsync();
        }



        public async Task<Exercise?> Get(Expression<Func<Exercise, bool>> criteria, IQueryable<Exercise> exercisesQueryable) => await exercisesQueryable.FirstOrDefaultAsync(criteria);

        public async Task<List<Exercise>> Get(int? offset = 0, int? limit = -1)
        {
            var exercises = GetIncluded()
                .Skip(offset ?? 0);

            if (limit != null && limit >= 0)
                exercises = exercises.Take(limit ?? 0);

            return await exercises.ToListAsync();
        }

        public async Task<List<Exercise>> Get(Expression<Func<Exercise, bool>> criteria, int offset = 0, int limit = -1)
        {
            var exercises = GetIncluded()
                .Where(criteria)
                .Skip(offset);

            if (limit >= 0)
                exercises = exercises.Take(limit);

            return await exercises.ToListAsync();
        }



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

        public AdvancedDTO<T> TranslateToAdvancedDTO<T>(List<T> values, string url, int offset, int limit)
        {
            AdvancedDTO<T> res = new()
            {
                BatchSize = values.Count,
                PreviousBatchURLExtension = null,
                NextBatchURLExtension = null,
                Values = values,
            };

            if (offset > 0)
            {
                var previousOffset = offset - limit;
                if (previousOffset < 0)
                    previousOffset = 0;

                res.PreviousBatchURLExtension = $"{url}&offset={previousOffset}&limit={limit}";
            }

            if (limit >= 0 && values.Count >= limit)
            {
                var nextOffset = offset + limit;

                res.NextBatchURLExtension = $"{url}&offset={nextOffset}&limit={limit}";
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