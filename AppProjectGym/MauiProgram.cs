﻿using AppProjectGym.Models;
using AppProjectGym.Pages;
using AppProjectGym.Services;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using AppProjectGym.Services.Read;
using AppProjectGym.Services.Create;
using AppProjectGym.Services.Update;
using AppProjectGym.Services.Delete;
using AppProjectGym.Services.Mapping;
using AppProjectGym.Information;
using System.Diagnostics;
using AppProjectGym.LocalDatabase;

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
            builder.Services.AddTransient<FullScreenExercisePage>();
            builder.Services.AddTransient<SearchResultsPage>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<ExerciseCreationPage>();
            builder.Services.AddTransient<MuscleCreationPage>();
            builder.Services.AddTransient<EquipmentCreationPage>();
            builder.Services.AddTransient<WorkoutCreationPage>();
            builder.Services.AddTransient<WorkoutEditPage>();
            builder.Services.AddTransient<UserWorkoutsPage>();
            builder.Services.AddTransient<StartedWorkoutPage>();

            builder.Services.AddTransient<IReadService, ReadService>();
            builder.Services.AddTransient<ICreateService, CreateService>();
            builder.Services.AddTransient<IUpdateService, UpdateService>();
            builder.Services.AddTransient<IDeleteService, DeleteService>();

            builder.Services.AddTransient<IEntityDisplayMapper<Exercise, ExerciseDisplay>, ExerciseDisplayMapper>();
            builder.Services.AddTransient<IEntityDisplayMapper<WorkoutSet, WorkoutSetDisplay>, WorkoutSetDisplayMapper>();
            builder.Services.AddTransient<IEntityDisplayMapper<WorkoutSet, StartedWorkout_SetDisplay>, StartedWorkoutSetDisplayMapper>();

            builder.Services.AddTransient<HttpClient>();

            return builder.Build();
        }
    }
}