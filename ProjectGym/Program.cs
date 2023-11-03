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
using ProjectGym.Services.Update;

namespace ProjectGym
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<ExerciseContext>();

            builder.Services.AddTransient<ICreateService<Exercise>, ExerciseCreateService>();
            builder.Services.AddTransient<IReadService<Exercise>, ExerciseReadService>();
            builder.Services.AddTransient<IDeleteService<Exercise>, DeleteService<Exercise>>();
            builder.Services.AddTransient<IEntityMapperAsync<Exercise, ExerciseDTO>, ExerciseMapper>();

            builder.Services.AddTransient<IReadService<User>, UserReadService>();
            builder.Services.AddTransient<ICreateService<User>, UserCreateService>();
            builder.Services.AddTransient<IEntityMapper<User, UserDTO>, UserMapper>();

            builder.Services.AddTransient<IReadService<Client>, ClientReadService>();
            builder.Services.AddTransient<ICreateService<Client>, ClientCreateService>();
            builder.Services.AddTransient<IUpdateService<Client>, ClientUpdateService>();

            builder.Services.AddTransient<IReadService<ExerciseVariation>, ExerciseVariationReadService>();
            builder.Services.AddTransient<IDeleteService<ExerciseVariation>, DeleteService<ExerciseVariation>>();

            builder.Services.AddTransient<IReadService<Muscle>, MuscleReadService>();
            builder.Services.AddTransient<IEntityMapper<Muscle, MuscleDTO>, MuscleMapper>();

            builder.Services.AddTransient<IReadService<EquipmentExerciseUsage>, EquipmentExerciseUsageReadService>();
            builder.Services.AddTransient<ICreateService<EquipmentExerciseUsage>, EquipmentExerciseUsageCreateService>();
            builder.Services.AddTransient<IDeleteService<EquipmentExerciseUsage>, DeleteService<EquipmentExerciseUsage>>();

            builder.Services.AddTransient<IReadService<Equipment>, EquipmentReadService>();
            builder.Services.AddTransient<ICreateService<Equipment>, EquipmentCreateService>();
            builder.Services.AddTransient<IUpdateService<Equipment>, EquipmentUpdateService>();
            builder.Services.AddTransient<IDeleteService<Equipment>, DeleteService<Equipment>>();
            builder.Services.AddTransient<IEntityMapperAsync<Equipment, EquipmentDTO>, EquipmentMapper>();

            builder.Services.AddTransient<IReadService<ExerciseAlias>, AliasReadService>();
            builder.Services.AddTransient<IEntityMapper<ExerciseAlias, ExerciseAliasDTO>, AliasMapper>();

            builder.Services.AddTransient<IReadService<ExerciseCategory>, CategoryReadService>();
            builder.Services.AddTransient<IEntityMapper<ExerciseCategory, CategoryDTO>, CategoryMapper>();

            builder.Services.AddTransient<IReadService<ExerciseNote>, NoteReadService>();
            builder.Services.AddTransient<IEntityMapper<ExerciseNote, NoteDTO>, NoteMapper>();

            builder.Services.AddTransient<IReadService<ExerciseImage>, ImageReadService>();

            builder.Services.AddTransient<IReadService<ExerciseVideo>, VideoReadService>();

            builder.Services.AddTransient<IReadService<PrimaryMuscleExerciseConnection>, PrimaryMuscleExerciseConnectionReadService>();
            builder.Services.AddTransient<ICreateService<PrimaryMuscleExerciseConnection>, PrimaryMuscleExerciseConnectionCreateService>();

            builder.Services.AddTransient<IReadService<SecondaryMuscleExerciseConnection>, SecondaryMuscleExerciseConnectionReadService>();
            builder.Services.AddTransient<ICreateService<SecondaryMuscleExerciseConnection>, SecondaryMuscleExerciseConnectionCreateService>();

            builder.Services.AddControllers();

            var app = builder.Build();
            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}