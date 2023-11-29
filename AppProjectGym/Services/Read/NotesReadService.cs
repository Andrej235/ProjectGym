using AppProjectGym.Models;

namespace AppProjectGym.Services.Read
{
    public class NotesReadService : AbstractReadService<Note>
    {
        public NotesReadService(HttpClient client) : base(client) { }

        protected override string URLExtension => "note";
    }
}
