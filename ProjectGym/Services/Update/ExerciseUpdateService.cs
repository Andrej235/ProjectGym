using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Update
{
    public class ExerciseUpdateService(ExerciseContext context,
                                       IReadService<Exercise> readService,
                                       IDeleteService<EquipmentUsage> equipmentUsageDeleteService,
                                       IDeleteService<PrimaryMuscleGroupInExercise> primaryMuscleGroupDeleteService,
                                       IDeleteService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupDeleteService,
                                       IDeleteService<PrimaryMuscleInExercise> primaryMuscleDeleteService,
                                       IDeleteService<SecondaryMuscleInExercise> secondaryMuscleDeleteService) : IUpdateService<Exercise>
    {
        public async Task Update(Exercise updatedEntity)
        {
            try
            {
                var exercise = await readService.Get(x => x.Id == updatedEntity.Id, "none") ?? throw new NullReferenceException("Exercise was not found.");
                context.AttachRange(exercise);

                if (updatedEntity.Equipment.Any())
                {
                    await equipmentUsageDeleteService.DeleteAll(x => x.ExerciseId == exercise.Id);
                    exercise.Equipment = updatedEntity.Equipment;
                }

                if (updatedEntity.PrimaryMuscleGroups.Any())
                {
                    await primaryMuscleGroupDeleteService.DeleteAll(x => x.ExerciseId == exercise.Id);
                    exercise.PrimaryMuscleGroups = updatedEntity.PrimaryMuscleGroups;
                }

                await secondaryMuscleGroupDeleteService.DeleteAll(x => x.ExerciseId == exercise.Id);
                exercise.SecondaryMuscleGroups = updatedEntity.SecondaryMuscleGroups;

                if (updatedEntity.PrimaryMuscles.Any())
                {
                    await primaryMuscleDeleteService.DeleteAll(x => x.ExerciseId == exercise.Id);
                    exercise.PrimaryMuscles = updatedEntity.PrimaryMuscles;
                }

                await secondaryMuscleDeleteService.DeleteAll(x => x.ExerciseId == exercise.Id);
                exercise.SecondaryMuscles = updatedEntity.SecondaryMuscles;

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
            }
        }
    }
}
