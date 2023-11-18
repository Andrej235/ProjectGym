using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
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

            #region Muscle group
            builder.Services.AddTransient<IReadService<MuscleGroup>, MuscleGroupReadService>();
            builder.Services.AddTransient<ICreateService<MuscleGroup, int>, MuscleGroupCreateService>();
            builder.Services.AddTransient<IEntityMapper<MuscleGroup, MuscleGroupDTO>, MuscleGroupMapper>();
            builder.Services.AddTransient<IEntityMapperSync<MuscleGroup, MuscleGroupDTO>, MuscleGroupMapper>();
            #endregion

            #region Muscle
            builder.Services.AddTransient<IReadService<Muscle>, MuscleReadService>();
            builder.Services.AddTransient<ICreateService<Muscle, int>, MuscleCreateService>();
            builder.Services.AddTransient<IEntityMapper<Muscle, MuscleDTO>, MuscleMapper>();
            builder.Services.AddTransient<IEntityMapperSync<Muscle, MuscleDTO>, MuscleMapper>();
            #endregion

            #region Equipment exercise usage
            builder.Services.AddTransient<ICreateService<EquipmentUsage, int>, EquipmentExerciseUsageCreateService>();
            builder.Services.AddTransient<IReadService<EquipmentUsage>, EquipmentExerciseUsageReadService>();
            builder.Services.AddTransient<IDeleteService<EquipmentUsage>, DeleteService<EquipmentUsage>>();
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
            builder.Services.AddTransient<IReadService<Alias>, AliasReadService>();
            builder.Services.AddTransient<ICreateService<Alias, int>, AliasCreateService>();
            builder.Services.AddTransient<IEntityMapper<Alias, ExerciseAliasDTO>, AliasMapper>();
            builder.Services.AddTransient<IEntityMapperSync<Alias, ExerciseAliasDTO>, AliasMapper>();
            #endregion

            #region Note
            builder.Services.AddTransient<IReadService<Note>, NoteReadService>();
            builder.Services.AddTransient<ICreateService<Note, int>, NoteCreateService>();
            builder.Services.AddTransient<IEntityMapper<Note, NoteDTO>, NoteMapper>();
            builder.Services.AddTransient<IEntityMapperSync<Note, NoteDTO>, NoteMapper>();
            #endregion

            #region Image
            builder.Services.AddTransient<ICreateService<Image, int>, ImageCreateService>();
            builder.Services.AddTransient<IReadService<Image>, ImageReadService>();
            builder.Services.AddTransient<IEntityMapper<Image, ImageDTO>, ImageMapper>();
            builder.Services.AddTransient<IEntityMapperSync<Image, ImageDTO>, ImageMapper>();
            #endregion

            #region Primary muscle exercise connection
            builder.Services.AddTransient<ICreateService<PrimaryMuscleGroupInExercise, int>, PrimaryMuscleExerciseConnectionCreateService>();
            builder.Services.AddTransient<IReadService<PrimaryMuscleGroupInExercise>, PrimaryMuscleExerciseConnectionReadService>();
            #endregion

            #region Secondary muscle exercise connection
            builder.Services.AddTransient<IReadService<SecondaryMuscleGroupInExercise>, SecondaryMuscleExerciseConnectionReadService>();
            builder.Services.AddTransient<ICreateService<SecondaryMuscleGroupInExercise, int>, SecondaryMuscleExerciseConnectionCreateService>();
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