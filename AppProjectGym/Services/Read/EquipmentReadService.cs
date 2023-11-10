using AppProjectGym.Models;

namespace AppProjectGym.Services.Read
{
    public class EquipmentReadService : AbstractReadService<Equipment>
    {
        public EquipmentReadService(HttpClient client) : base(client) { }

        protected override string URLExtension => "equipment";
    }
}
