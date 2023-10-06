using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using static ProjectGym.Controllers.ExerciseController;
using System.Text.RegularExpressions;

namespace ProjectGym.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly ExerciseContext context = new();
        public enum IncludeType
        {
            None,
            Basic,
            All
        }


        public IQueryable<Exercise> GetIncluding(IncludeType includeType = IncludeType.All)
        {
            return includeType switch
            {
                IncludeType.None => context.Exercises,
                IncludeType.Basic => context.Exercises.Include(e => e.Images),
                IncludeType.All => context.Exercises
                .Include(e => e.VariationExercises)
                .Include(e => e.IsVariationOf)
                .Include(e => e.Category)
                .Include(e => e.PrimaryMuscles)
                .Include(e => e.SecondaryMuscles)
                .Include(e => e.Equipment)
                .Include(e => e.Videos)
                .Include(e => e.Aliases)
                .Include(e => e.Notes)
                .Include(e => e.Images),
                _ => context.Exercises
            };
        }





        [HttpGet("querytest")]
        public async Task<IActionResult> QueryTesting([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include)
        {
            IQueryable<Exercise> exercisesIncluding;
            if (include != null)
            {
                string[] toInclude = include.ToLower().Replace(" ", "").Split(',');
                if (toInclude.Contains("all"))
                {
                    exercisesIncluding = GetIncluding(IncludeType.All);
                }
                else if (toInclude.Contains("none"))
                {
                    exercisesIncluding = GetIncluding(IncludeType.None);
                }
                else
                {
                    exercisesIncluding = context.Exercises.AsQueryable();
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
            }
            else
            {
                exercisesIncluding = GetIncluding(IncludeType.All);
            }


            string url = "/api/exercise/querytest";
            
            if (include != null)
                url += $"?include={include}";

            return Ok(TranslateToAdvancedDTO(TranslateToDTO(await Get(exercisesIncluding, offset, limit)), url, offset ?? 0, limit ?? -1));
        }



        public async Task<List<Exercise>> Get(IQueryable<Exercise> exercisesQueryable, int? offset = 0, int? limit = -1)
        {
            exercisesQueryable = exercisesQueryable.Skip(offset ?? 0);

            if (limit != null && limit >= 0)
                exercisesQueryable = exercisesQueryable.Take(limit ?? 0);

            return await exercisesQueryable.ToListAsync();
        }

        public async Task<List<Exercise>> Get(int? offset = 0, int? limit = -1, IncludeType includeType = IncludeType.All)
        {
            var exercises = GetIncluding(includeType)
                .Skip(offset ?? 0);

            if (limit != null && limit >= 0)
                exercises = exercises.Take(limit ?? 0);

            return await exercises.ToListAsync();
        }

        public async Task<List<Exercise>> Get(Expression<Func<Exercise, bool>> criteria, int offset = 0, int limit = -1)
        {
            var exercises = GetIncluding()
                .Where(criteria)
                .Skip(offset);

            if (limit >= 0)
                exercises = exercises.Take(limit);

            return await exercises.ToListAsync();
        }



        public ExerciseDTO TranslateToDTO(Exercise exercise) => new()
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




        [HttpGet("basic")]
        public async Task<IActionResult> A() => Ok(TranslateToDTO(await GetIncluding(IncludeType.Basic).ToListAsync()));
        [HttpGet]
        public async Task<IActionResult> GetFull() => Ok(TranslateToDTO(await GetIncluding().Take(10).ToListAsync()));






        public class AdvancedDTO<T>
        {
            public int BatchSize { get; set; }
            public string? PreviousBatchURLExtension { get; set; }
            public string? NextBatchURLExtension { get; set; }
            public List<T> Values { get; set; } = null!;
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
}
