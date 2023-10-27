using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class EquipmentReadService : AbstractReadService<Equipment, int>
    {
        private readonly ExerciseContext context;
        public EquipmentReadService(ExerciseContext context)
        {
            this.context = context;
        }

        protected override Func<Equipment, int> PrimaryKey => eq => eq.Id;

        protected override IQueryable<Equipment> GetIncluded(IEnumerable<string>? include)
        {
            IQueryable<Equipment> equipmentIncluding = context.Equipment.AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return equipmentIncluding;

            if (include.Contains("all") || include.Contains("exercises"))
                return equipmentIncluding.Include(eq => eq.UsedInExercises);

            return equipmentIncluding;
        }

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
