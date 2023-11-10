using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Services.Read
{
    public class ImageReadService : AbstractReadService<Image>
    {
        public ImageReadService(HttpClient client) : base(client) { }

        protected override string URLExtension => "image";
    }
}
