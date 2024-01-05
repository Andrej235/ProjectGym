using ProjectGym.Services.DatabaseSerialization;
using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Models
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }
        public User? User { get; set; } = null!;
        [ModelReference("User")]
        public Guid? UserGUID { get; set; }
    }
}
