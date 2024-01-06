using AppProjectGym.Information;
using AppProjectGym.LocalDatabase;
using AppProjectGym.Pages;
using AppProjectGym.Services;

namespace AppProjectGym
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            InitializeLocalDB();
            MainPage = new AppShell();

            InitializeLocalDB();
            LoadUser();
        }

        private static async void LoadUser()
        {
            if (!await ClientInfo.SetUser())
                await NavigationService.GoToAsync(nameof(LoginPage));
        }
        private static async void InitializeLocalDB() => await new FinishedWorkoutContext(new()).Initialize();
    }
}