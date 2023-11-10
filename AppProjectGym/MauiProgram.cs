using AppProjectGym.Models;
using AppProjectGym.Pages;
using AppProjectGym.Services;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using AppProjectGym.Information;
using AppProjectGym.Services.Read;
using Image = AppProjectGym.Models.Image;

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

            builder.Services.AddTransient<IReadService<Exercise>, ExerciseReadService>();
            builder.Services.AddTransient<IReadService<Image>, ImageReadService>();
            builder.Services.AddTransient<IReadService<ExerciseNote>, NotesReadService>();
            builder.Services.AddTransient<IReadService<Equipment>, EquipmentReadService>();
            builder.Services.AddTransient<IReadService<ExerciseCategory>, CategoryReadService>();
            builder.Services.AddTransient<IReadService<Muscle>, MuscleReadService>();

            builder.Services.AddTransient<ExerciseDisplayMapper>();

            builder.Services.AddTransient<HttpClient>();

            return builder.Build();
        }
    }
}