using AppProjectGym.Models;

namespace AppProjectGym.Services.Create
{
    public class ExerciseCreateService : AbstractCreateService<Exercise, int>
    {
        public ExerciseCreateService(HttpClient client) : base(client) { }

        protected override string URLExtension => "exercise";

        protected override Func<string, int> ParsePrimaryKey => int.Parse;
    }
}
