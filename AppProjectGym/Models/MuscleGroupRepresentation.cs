namespace AppProjectGym.Models
{
    public record class MuscleGroupRepresentation
    {
        public MuscleGroupDisplay MuscleGroupDisplay { get; set; }
        public List<Muscle> SelectedMuscles { get; set; }

        public MuscleGroupRepresentation(MuscleGroupDisplay MuscleGroupDisplay, List<Muscle> SelectedMuscles)
        {
            this.MuscleGroupDisplay = MuscleGroupDisplay;
            this.SelectedMuscles = SelectedMuscles;
        }

        public MuscleGroupRepresentation()
        {
            MuscleGroupDisplay = null;
            SelectedMuscles = [];
        }
    }
}
