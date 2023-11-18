using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class ClientReadService(ExerciseContext context) : AbstractReadService<Client, Guid>
    {
        protected override Func<Client, Guid> PrimaryKey => c => c.Id;

        protected override IQueryable<Client> GetIncluded(IEnumerable<string>? include)
        {
            IQueryable<Client> equipmentIncluding = context.Clients.AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return equipmentIncluding;

            if (include.Contains("all") || include.Contains("user"))
                return equipmentIncluding.Include(eq => eq.User);

            return equipmentIncluding;
        }

        protected override Expression<Func<Client, bool>> TranslateKeyValueToExpression(string key, string value) => x => false;
    }
}
