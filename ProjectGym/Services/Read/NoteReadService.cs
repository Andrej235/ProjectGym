using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class NoteReadService(ExerciseContext context) : ReadService<Note>(context)
    {
        protected override Expression<Func<Note, bool>> TranslateKeyValueToExpression(string key, string value) => x => false;
    }
}
