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

            #region Exercise
            builder.Services.AddTransient<ICreateService<Exercise, int>, ExerciseCreateService>();
            builder.Services.AddTransient<IReadService<Exercise>, ExerciseReadService>();
            builder.Services.AddTransient<IDeleteService<Exercise>, DeleteService<Exercise>>();
            builder.Services.AddTransient<IEntityMapper<Exercise, ExerciseDTO>, ExerciseMapper>();
            builder.Services.AddTransient<IEntityMapperAsync<Exercise, ExerciseDTO>, ExerciseMapper>();
            #endregion

            #region User
            builder.Services.AddTransient<IReadService<User>, UserReadService>();
            builder.Services.AddTransient<ICreateService<User, Guid>, UserCreateService>();
            builder.Services.AddTransient<IEntityMapper<User, UserDTO>, UserMapper>();
            #endregion

            #region Client
            builder.Services.AddTransient<IReadService<Client>, ClientReadService>();
            builder.Services.AddTransient<ICreateService<Client, Guid>, ClientCreateService>();
            builder.Services.AddTransient<IUpdateService<Client>, ClientUpdateService>();
            #endregion

            #region Exercise Variation
            builder.Services.AddTransient<IReadService<ExerciseVariation>, ExerciseVariationReadService>();
            builder.Services.AddTransient<IDeleteService<ExerciseVariation>, DeleteService<ExerciseVariation>>();
            #endregion

            #region Muscle
            builder.Services.AddTransient<IReadService<Muscle>, MuscleReadService>();
            builder.Services.AddTransient<ICreateService<Muscle, int>, MuscleCreateService>();
            builder.Services.AddTransient<IEntityMapper<Muscle, MuscleDTO>, MuscleMapper>();
            builder.Services.AddTransient<IEntityMapperAsync<Muscle, MuscleDTO>, MuscleMapper>();
            #endregion

            #region Equipment exercise usage
            builder.Services.AddTransient<ICreateService<EquipmentExerciseUsage, int>, EquipmentExerciseUsageCreateService>();
            builder.Services.AddTransient<IReadService<EquipmentExerciseUsage>, EquipmentExerciseUsageReadService>();
            builder.Services.AddTransient<IDeleteService<EquipmentExerciseUsage>, DeleteService<EquipmentExerciseUsage>>();
            #endregion

            #region Equipment
            builder.Services.AddTransient<ICreateService<Equipment, int>, EquipmentCreateService>();
            builder.Services.AddTransient<IReadService<Equipment>, EquipmentReadService>();
            builder.Services.AddTransient<IUpdateService<Equipment>, EquipmentUpdateService>();
            builder.Services.AddTransient<IDeleteService<Equipment>, DeleteService<Equipment>>();
            builder.Services.AddTransient<IEntityMapper<Equipment, EquipmentDTO>, EquipmentMapper>();
            builder.Services.AddTransient<IEntityMapperAsync<Equipment, EquipmentDTO>, EquipmentMapper>();
            #endregion

            #region Alias
            builder.Services.AddTransient<IReadService<ExerciseAlias>, AliasReadService>();
            builder.Services.AddTransient<ICreateService<ExerciseAlias, int>, AliasCreateService>();
            builder.Services.AddTransient<IEntityMapper<ExerciseAlias, ExerciseAliasDTO>, AliasMapper>();
            builder.Services.AddTransient<IEntityMapperSync<ExerciseAlias, ExerciseAliasDTO>, AliasMapper>();
            #endregion

            #region Category
            builder.Services.AddTransient<IReadService<ExerciseCategory>, CategoryReadService>();
            builder.Services.AddTransient<ICreateService<ExerciseCategory, int>, CategoryCreateService>();
            builder.Services.AddTransient<IEntityMapper<ExerciseCategory, CategoryDTO>, CategoryMapper>();
            builder.Services.AddTransient<IEntityMapperAsync<ExerciseCategory, CategoryDTO>, CategoryMapper>();
            #endregion

            #region Note
            builder.Services.AddTransient<IReadService<ExerciseNote>, NoteReadService>();
            builder.Services.AddTransient<ICreateService<ExerciseNote, int>, NoteCreateService>();
            builder.Services.AddTransient<IEntityMapper<ExerciseNote, NoteDTO>, NoteMapper>();
            builder.Services.AddTransient<IEntityMapperSync<ExerciseNote, NoteDTO>, NoteMapper>();
            #endregion

            #region Image
            builder.Services.AddTransient<ICreateService<ExerciseImage, int>, ImageCreateService>();
            builder.Services.AddTransient<IReadService<ExerciseImage>, ImageReadService>();
            builder.Services.AddTransient<IEntityMapper<ExerciseImage, ImageDTO>, ImageMapper>();
            builder.Services.AddTransient<IEntityMapperSync<ExerciseImage, ImageDTO>, ImageMapper>();
            #endregion

            #region Video
            builder.Services.AddTransient<IReadService<ExerciseVideo>, VideoReadService>();
            #endregion

            #region Primary muscle exercise connection
            builder.Services.AddTransient<ICreateService<PrimaryMuscleExerciseConnection, int>, PrimaryMuscleExerciseConnectionCreateService>();
            builder.Services.AddTransient<IReadService<PrimaryMuscleExerciseConnection>, PrimaryMuscleExerciseConnectionReadService>();
            #endregion

            #region Secondary muscle exercise connection
            builder.Services.AddTransient<IReadService<SecondaryMuscleExerciseConnection>, SecondaryMuscleExerciseConnectionReadService>();
            builder.Services.AddTransient<ICreateService<SecondaryMuscleExerciseConnection, int>, SecondaryMuscleExerciseConnectionCreateService>();
            #endregion

            builder.Services.AddControllers();

            var app = builder.Build();
            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}