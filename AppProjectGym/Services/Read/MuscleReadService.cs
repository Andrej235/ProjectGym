using AppProjectGym.Models;

namespace AppProjectGym.Services.Read
{
    public class MuscleReadService : AbstractReadService<Muscle>
    {
        public MuscleReadService(HttpClient client) : base(client) { }

        protected override string URLExtension => "muscle";
    }
}
