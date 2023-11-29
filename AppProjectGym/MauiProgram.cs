using AppProjectGym.Models;
using AppProjectGym.Pages;
using AppProjectGym.Services;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using AppProjectGym.Services.Read;
using Image = AppProjectGym.Models.Image;
using AppProjectGym.Services.Create;

namespace AppProjectGym
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<FullScreenExercise>();
            builder.Services.AddTransient<SearchResultsPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<ExerciseCreationPage>();

            builder.Services.AddTransient<IReadService, ReadService>();

            builder.Services.AddTransient<ICreateService, CreateService>();

            builder.Services.AddTransient<ExerciseDisplayMapper>();

            builder.Services.AddTransient<HttpClient>();

            return builder.Build();
        }
    }
}