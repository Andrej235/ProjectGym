using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class ImageReadService(ExerciseContext context) : ReadService<Image>(context)
    {
        protected override Expression<Func<Image, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key != "exercise")
                throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");

            if (int.TryParse(value, out int id))
                return x => x.ExerciseId == id;

            throw new NotSupportedException($"Invalid search query value. Entered value: {value}");
        }
    }
}
