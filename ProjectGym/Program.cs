using Microsoft.Identity.Client;
using ProjectGym.Controllers;
using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;

namespace ProjectGym
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<ExerciseContext>();

            builder.Services.AddTransient<IReadService<Exercise>, ExerciseReadService>();
            builder.Services.AddTransient<IEntityMapper<Exercise, ExerciseDTO>, ExerciseMapper>();

            builder.Services.AddTransient<IReadService<Muscle>, MuscleReadService>();
            builder.Services.AddTransient<IEntityMapper<Muscle, MuscleDTO>, MuscleMapper>();
            
            builder.Services.AddTransient<IReadService<Equipment>, EquipmentReadService>();
            builder.Services.AddTransient<ICreateService<Equipment>, EquipmentCreateService>();
            builder.Services.AddTransient<IDeleteService<Equipment>, DeleteService<Equipment>>();
            builder.Services.AddTransient<IEntityMapperAsync<Equipment, EquipmentDTO>, EquipmentMapper>();

            builder.Services.AddTransient<IReadService<ExerciseAlias>, AliasReadService>();
            builder.Services.AddTransient<IEntityMapper<ExerciseAlias, ExerciseAliasDTO>, AliasMapper>();

            builder.Services.AddTransient<IReadService<ExerciseCategory>, CategoryReadService>();
            builder.Services.AddTransient<IEntityMapper<ExerciseCategory, CategoryDTO>, CategoryMapper>();

            builder.Services.AddTransient<IReadService<ExerciseNote>, NoteReadService>();
            builder.Services.AddTransient<IEntityMapper<ExerciseNote, NoteDTO>, NoteMapper>();

            builder.Services.AddControllers();

            var app = builder.Build();
            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}