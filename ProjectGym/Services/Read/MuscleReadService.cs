using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class MuscleReadService(ExerciseContext context) : ReadService<Muscle>(context)
    {
        protected override Expression<Func<Muscle, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return x => x.Name.ToLower().Contains(value.ToLower());
            }

            if (int.TryParse(value, out var id))
            {
                return key switch
                {
                    "musclegroup" => x => x.MuscleGroupId == id,
                    "primaryin" => x => x.PrimaryInExercises.Any(x => x.Id == id),
                    "primary" => x => x.PrimaryInExercises.Any(x => x.Id == id),
                    "secondaryin" => x => x.SecondaryInExercises.Any(x => x.Id == id),
                    "secondary" => x => x.SecondaryInExercises.Any(x => x.Id == id),
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}")
                };
            }
            throw new NotSupportedException($"Invalid value in search query. Entered value '{value}' for key '{key}'");
        }
    }
}
