using Microsoft.Extensions.Options;
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

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost" || new Uri(origin).Host == "192.168.1.100")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            builder.Services.AddTransient<ExerciseContext>();

            #region Exercise
            builder.Services.AddTransient<ICreateService<Exercise>, ExerciseCreateService>();
            builder.Services.AddTransient<IReadService<Exercise>, ExerciseReadService>();
            builder.Services.AddTransient<IUpdateService<Exercise>, ExerciseUpdateService>();
            builder.Services.AddTransient<IDeleteService<Exercise>, DeleteService<Exercise>>();
            builder.Services.AddTransient<IEntityMapper<Exercise, ExerciseDTO>, ExerciseMapper>();
            builder.Services.AddTransient<IEntityMapperAsync<Exercise, ExerciseDTO>, ExerciseMapper>();
            #endregion

            #region User
            builder.Services.AddTransient<IReadService<User>, UserReadService>();
            builder.Services.AddTransient<ICreateService<User>, UserCreateService>();
            builder.Services.AddTransient<IEntityMapper<User, UserDTO>, UserMapper>();
            #endregion

            #region Weight
            builder.Services.AddTransient<ICreateService<PersonalExerciseWeight>, WeightCreateService>();
            builder.Services.AddTransient<IReadService<PersonalExerciseWeight>, WeightReadService>();
            builder.Services.AddTransient<IUpdateService<PersonalExerciseWeight>, UpdateService<PersonalExerciseWeight>>();
            builder.Services.AddTransient<IDeleteService<PersonalExerciseWeight>, DeleteService<PersonalExerciseWeight>>();
            builder.Services.AddTransient<IEntityMapper<PersonalExerciseWeight, PersonalExerciseWeightDTO>, WeightMapper>();
            #endregion

            #region Set
            builder.Services.AddTransient<ICreateService<Set>, CreateService<Set>>();
            builder.Services.AddTransient<IReadService<Set>, SetReadService>();
            builder.Services.AddTransient<IUpdateService<Set>, UpdateService<Set>>();
            builder.Services.AddTransient<IDeleteService<Set>, DeleteService<Set>>();
            builder.Services.AddTransient<IEntityMapper<Set, SetDTO>, SetMapper>();
            #endregion

            #region Workout Set
            builder.Services.AddTransient<ICreateService<WorkoutSet>, WorkoutSetCreateService>();
            builder.Services.AddTransient<IReadService<WorkoutSet>, WorkoutSetReadService>();
            builder.Services.AddTransient<IUpdateService<WorkoutSet>, UpdateService<WorkoutSet>>();
            builder.Services.AddTransient<IDeleteService<WorkoutSet>, DeleteService<WorkoutSet>>();
            builder.Services.AddTransient<IEntityMapper<WorkoutSet, WorkoutSetDTO>, WorkoutSetMapper>();
            #endregion

            #region Workouts
            builder.Services.AddTransient<IReadService<Workout>, WorkoutReadService>();
            builder.Services.AddTransient<ICreateService<Workout>, CreateService<Workout>>();
            builder.Services.AddTransient<IUpdateService<Workout>, WorkoutUpdateService>();
            builder.Services.AddTransient<IDeleteService<Workout>, DeleteService<Workout>>();
            builder.Services.AddTransient<IEntityMapper<Workout, WorkoutDTO>, WorkoutMapper>();
            #endregion

            #region Client
            builder.Services.AddTransient<IReadService<Client>, ClientReadService>();
            builder.Services.AddTransient<ICreateService<Client>, ClientCreateService>();
            builder.Services.AddTransient<IUpdateService<Client>, ClientUpdateService>();
            #endregion

            #region Muscle group
            builder.Services.AddTransient<ICreateService<MuscleGroup>, MuscleGroupCreateService>();
            builder.Services.AddTransient<IReadService<MuscleGroup>, MuscleGroupReadService>();
            builder.Services.AddTransient<IUpdateService<MuscleGroup>, UpdateService<MuscleGroup>>();
            builder.Services.AddTransient<IDeleteService<MuscleGroup>, DeleteService<MuscleGroup>>();
            builder.Services.AddTransient<IEntityMapper<MuscleGroup, MuscleGroupDTO>, MuscleGroupMapper>();
            #endregion

            #region Muscle
            builder.Services.AddTransient<ICreateService<Muscle>, CreateService<Muscle>>();
            builder.Services.AddTransient<IReadService<Muscle>, MuscleReadService>();
            builder.Services.AddTransient<IUpdateService<Muscle>, UpdateService<Muscle>>();
            builder.Services.AddTransient<IDeleteService<Muscle>, DeleteService<Muscle>>();
            builder.Services.AddTransient<IEntityMapper<Muscle, MuscleDTO>, MuscleMapper>();
            #endregion

            #region Equipment exercise usage
            builder.Services.AddTransient<ICreateService<EquipmentUsage>, EquipmentExerciseUsageCreateService>();
            builder.Services.AddTransient<IReadService<EquipmentUsage>, EquipmentExerciseUsageReadService>();
            builder.Services.AddTransient<IDeleteService<EquipmentUsage>, DeleteService<EquipmentUsage>>();
            #endregion

            #region Equipment
            builder.Services.AddTransient<ICreateService<Equipment>, EquipmentCreateService>();
            builder.Services.AddTransient<IReadService<Equipment>, EquipmentReadService>();
            builder.Services.AddTransient<IUpdateService<Equipment>, UpdateService<Equipment>>();
            builder.Services.AddTransient<IDeleteService<Equipment>, DeleteService<Equipment>>();
            builder.Services.AddTransient<IEntityMapper<Equipment, EquipmentDTO>, EquipmentMapper>();
            #endregion

            #region Alias
            builder.Services.AddTransient<IReadService<Alias>, AliasReadService>();
            builder.Services.AddTransient<ICreateService<Alias>, AliasCreateService>();
            builder.Services.AddTransient<IUpdateService<Alias>, UpdateService<Alias>>();
            builder.Services.AddTransient<IDeleteService<Alias>, DeleteService<Alias>>();
            builder.Services.AddTransient<IEntityMapper<Alias, AliasDTO>, AliasMapper>();
            #endregion

            #region Note
            builder.Services.AddTransient<IReadService<Note>, NoteReadService>();
            builder.Services.AddTransient<ICreateService<Note>, NoteCreateService>();
            builder.Services.AddTransient<IUpdateService<Note>, UpdateService<Note>>();
            builder.Services.AddTransient<IDeleteService<Note>, DeleteService<Note>>();
            builder.Services.AddTransient<IEntityMapper<Note, NoteDTO>, NoteMapper>();
            #endregion

            #region Image
            builder.Services.AddTransient<ICreateService<Image>, ImageCreateService>();
            builder.Services.AddTransient<IReadService<Image>, ImageReadService>();
            builder.Services.AddTransient<IUpdateService<Image>, UpdateService<Image>>();
            builder.Services.AddTransient<IDeleteService<Image>, DeleteService<Image>>();
            builder.Services.AddTransient<IEntityMapper<Image, ImageDTO>, ImageMapper>();
            #endregion

            #region Primary muscle group
            builder.Services.AddTransient<ICreateService<PrimaryMuscleGroupInExercise>, PrimaryMuscleExerciseConnectionCreateService>();
            builder.Services.AddTransient<IReadService<PrimaryMuscleGroupInExercise>, PrimaryMuscleGroupReadService>();
            builder.Services.AddTransient<IDeleteService<PrimaryMuscleGroupInExercise>, DeleteService<PrimaryMuscleGroupInExercise>>();
            #endregion

            #region Secondary muscle group
            builder.Services.AddTransient<IReadService<SecondaryMuscleGroupInExercise>, SecondaryMuscleGroupReadService>();
            builder.Services.AddTransient<ICreateService<SecondaryMuscleGroupInExercise>, SecondaryMuscleExerciseConnectionCreateService>();
            builder.Services.AddTransient<IDeleteService<SecondaryMuscleGroupInExercise>, DeleteService<SecondaryMuscleGroupInExercise>>();
            #endregion

            #region Primary muscle
            builder.Services.AddTransient<IReadService<PrimaryMuscleInExercise>, PrimaryMuscleReadService>();
            builder.Services.AddTransient<IDeleteService<PrimaryMuscleInExercise>, DeleteService<PrimaryMuscleInExercise>>();
            #endregion

            #region Secondary muscle
            builder.Services.AddTransient<IReadService<SecondaryMuscleInExercise>, SecondaryMuscleReadService>();
            builder.Services.AddTransient<IDeleteService<SecondaryMuscleInExercise>, DeleteService<SecondaryMuscleInExercise>>();
            #endregion

            #region Bookmarks
            builder.Services.AddTransient<ICreateService<ExerciseBookmark>, BookmarkCreateService>();
            builder.Services.AddTransient<IReadService<ExerciseBookmark>, BookmarkReadService>();
            builder.Services.AddTransient<IDeleteService<ExerciseBookmark>, DeleteService<ExerciseBookmark>>();
            #endregion

            builder.Services.AddControllers();

            var app = builder.Build();

            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors();
            app.Run();
        }
    }
}