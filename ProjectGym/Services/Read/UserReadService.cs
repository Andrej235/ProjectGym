using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class UserReadService(ExerciseContext context) : ReadService<User>(context)
    {
        protected override Expression<Func<User, bool>> TranslateKeyValueToExpression(string key, string value) => u => false;
    }
}
