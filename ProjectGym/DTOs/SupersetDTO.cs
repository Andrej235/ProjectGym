namespace ProjectGym.DTOs
{
    public class SupersetDTO
    {
        public Guid Id { get; set; }
        public int TargetSets { get; set; }
        public bool DropSets { get; set; } = false;
        public Guid SetId { get; set; }
    }
}
