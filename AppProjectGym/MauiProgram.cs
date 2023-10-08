using AppProjectGym.Models;
using AppProjectGym.Pages;
using AppProjectGym.Services;
using Microsoft.Extensions.Logging;

namespace AppProjectGym
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
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

            builder.Services.AddSingleton<IDataService<Exercise>, ExerciseDataService>();
            builder.Services.AddSingleton<IDataService<Muscle>, MuscleDataService>();
            builder.Services.AddSingleton<IDataService<ExerciseCategory>, ExerciseCategoryDataService>();
            builder.Services.AddSingleton<IDataService<ExerciseNote>, NotesDataService>();

            return builder.Build();
        }
    }
}