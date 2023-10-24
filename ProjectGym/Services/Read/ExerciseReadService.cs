using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class ExerciseReadService : AbstractReadService<Exercise, int>
    {
        private readonly ExerciseContext context;
        public ExerciseReadService(ExerciseContext context)
        {
            this.context = context;
        }

        protected override Func<Exercise, int> PrimaryKey => x => x.Id;

        protected override IQueryable<Exercise> GetIncluded(IEnumerable<string>? include)
        {
            IQueryable<Exercise> exercisesIncluding = context.Exercises.AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return exercisesIncluding;

            if (include.Contains("all"))
            {
                return exercisesIncluding
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
            else
            {
                foreach (var inc in include)
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

        protected override Expression<Func<Exercise, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                /*                var ids = exercisesQueryable.AsEnumerable().Where(e => e.Name.IsSimilar(value)).Select(e => e.Id);
                                return e => ids.Contains(e.Id);*/

                return e => e.Name.ToLower().Contains(value.ToLower());
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
}
