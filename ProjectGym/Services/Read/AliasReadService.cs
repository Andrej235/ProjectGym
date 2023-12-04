using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Mapping;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class AliasReadService(ExerciseContext context) : ReadService<Alias>(context)
    {
        protected override Expression<Func<Alias, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "exercise" && int.TryParse(value, out var id))
                return x => x.ExerciseId == id;

            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return x => x.AliasName.ToLower().Contains(value.ToLower());
            }

            throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
        }
    }
}
