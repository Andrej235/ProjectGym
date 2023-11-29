using AppProjectGym.Models;

namespace AppProjectGym.Services.Read
{
    public class ExerciseReadService(HttpClient client) : AbstractReadService<Exercise>(client)
    {
        protected override string URLExtension => "exercise";
    }
}
