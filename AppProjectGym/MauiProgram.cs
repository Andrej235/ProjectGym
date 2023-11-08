﻿using AppProjectGym.Models;
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

            builder.Services.AddSingleton<IDataService<Muscle>, MuscleDataService>();
            builder.Services.AddSingleton<IDataService<ExerciseCategory>, ExerciseCategoryDataService>();
            builder.Services.AddSingleton<IDataService<ExerciseNote>, NotesDataService>();
            builder.Services.AddSingleton<IDataService<Equipment>, EquipmentDataService>();

            builder.Services.AddTransient<IReadService<Exercise>, ExerciseReadService>();
            builder.Services.AddTransient<IReadService<Image>, ImageReadService>();

            return builder.Build();
        }
    }
}