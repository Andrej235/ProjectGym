using AppProjectGym.Models;

namespace AppProjectGym.Services.Read
{
    public class ExerciseReadService : AbstractReadService<Exercise>
    {
        public ExerciseReadService(HttpClient client) : base(client) { }

        protected override string URLExtension => "exercise";
    }
}
