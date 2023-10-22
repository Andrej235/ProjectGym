using System.ComponentModel.DataAnnotations;

namespace ProjectGym.Models
{
    public class Superset
    {
        [Key]
        public Guid Id { get; set; }
        public int TargetSets { get; set; }
        public bool DropSets { get; set; }

        public Set Set { get; set; } = null!;
        public Guid SetId { get; set; }
    }
}
