using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Services.Create
{
    public class ImageCreateService : AbstractCreateService<Image, int>
    {
        public ImageCreateService(HttpClient client) : base(client) { }

        protected override string URLExtension => "image";

        protected override Func<string, int> ParsePrimaryKey => int.Parse;
    }
}
