using AppProjectGym.Models;

namespace AppProjectGym.Services.Create
{
    public class ExerciseCreateService : AbstractCreateService<Exercise>
    {
        public ExerciseCreateService(HttpClient client) : base(client) { }

        protected override string URLExtension => "exercise";
    }
}
