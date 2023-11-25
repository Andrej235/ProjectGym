using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class EquipmentReadService(ExerciseContext context) : ReadService<Equipment>(context)
    {
        protected override Expression<Func<Equipment, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return eq => eq.Name.ToLower().Contains(value.ToLower());
            }
            throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
        }
    }
}
