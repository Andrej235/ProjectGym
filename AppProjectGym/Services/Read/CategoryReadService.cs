using AppProjectGym.Models;

namespace AppProjectGym.Services.Read
{
    public class CategoryReadService : AbstractReadService<ExerciseCategory>
    {
        public CategoryReadService(HttpClient client) : base(client) { }

        protected override string URLExtension => "category";
    }
}
