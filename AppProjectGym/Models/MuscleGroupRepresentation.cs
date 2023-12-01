namespace AppProjectGym.Models
{
    public record class MuscleGroupRepresentation
    {
        private List<Muscle> selectedMuscles;

        public MuscleGroupDisplay MuscleGroupDisplay { get; set; }
        public List<Muscle> SelectedMuscles
        {
            get => selectedMuscles; 
            set
            {
                selectedMuscles = value;
            }
        }

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
