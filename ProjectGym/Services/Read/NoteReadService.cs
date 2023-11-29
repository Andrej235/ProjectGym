using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public class NoteReadService(ExerciseContext context) : ReadService<Note>(context)
    {
        protected override Expression<Func<Note, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "exercise" && int.TryParse(value, out var id))
                return x => x.ExerciseId == id;

            throw new NotSupportedException($"Invalid value in search query. Entered value '{value}' for key '{key}'");
        }
    }
}
