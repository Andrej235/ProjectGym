using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class ClientReadService(ExerciseContext context) : ReadService<Client>(context)
    {
        protected override Expression<Func<Client, bool>> TranslateKeyValueToExpression(string key, string value) => x => false;
    }
}
