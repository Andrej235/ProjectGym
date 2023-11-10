using AppProjectGym.Models;

namespace AppProjectGym.Services.Read
{
    public class NotesReadService : AbstractReadService<ExerciseNote>
    {
        public NotesReadService(HttpClient client) : base(client) { }

        protected override string URLExtension => "note";
    }
}
