using AppProjectGym.Pages;

namespace AppProjectGym
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(FullScreenExercise), typeof(FullScreenExercise));
            Routing.RegisterRoute(nameof(SearchResultsPage), typeof(SearchResultsPage));
        }
    }
}