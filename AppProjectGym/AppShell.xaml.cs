using AppProjectGym.Pages;

namespace AppProjectGym
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(FullScreenExercisePage), typeof(FullScreenExercisePage));
            Routing.RegisterRoute(nameof(SearchResultsPage), typeof(SearchResultsPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(ExerciseCreationPage), typeof(ExerciseCreationPage));
            Routing.RegisterRoute(nameof(MuscleCreationPage), typeof(MuscleCreationPage));
            Routing.RegisterRoute(nameof(EquipmentCreationPage), typeof(EquipmentCreationPage));
            Routing.RegisterRoute(nameof(WorkoutCreationPage), typeof(WorkoutCreationPage));
            Routing.RegisterRoute(nameof(WorkoutEditPage), typeof(WorkoutEditPage));
            Routing.RegisterRoute(nameof(UserWorkoutsPage), typeof(UserWorkoutsPage));
            Routing.RegisterRoute(nameof(StartedWorkoutPage), typeof(StartedWorkoutPage));
        }
    }
}