using Microsoft.Identity.Client;
using ProjectGym.Controllers;
using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Mapping;

namespace ProjectGym
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddTransient<ExerciseContext>();
            builder.Services.AddTransient<ExerciseService>();
            builder.Services.AddTransient<BasicGetDataService<Muscle>>();
            builder.Services.AddTransient<BasicGetDataService<Equipment>>();
            builder.Services.AddTransient<BasicGetDataService<ExerciseCategory>>();
            builder.Services.AddTransient<BasicGetDataService<ExerciseAlias>>();
            builder.Services.AddTransient<BasicGetDataService<ExerciseNote>>();

            builder.Services.AddTransient<IReadService<Exercise>, ExerciseService>();

            builder.Services.AddTransient<IEntityMapper<Exercise, ExerciseDTO>, ExerciseMapper>();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}