using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Models
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }
        public User? User { get; set; } = null!;
        public Guid? UserGUID { get; set; }
    }
}
