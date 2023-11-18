using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Update
{
    public class ExerciseUpdateService(ExerciseContext context, IReadService<Exercise> readService) : IUpdateService<Exercise>
    {
        public async Task Update(Exercise updatedEntity)
        {
            try
            {
                var exercise = await readService.Get(x => x.Id == updatedEntity.Id, "none") ?? throw new NullReferenceException("Exercise was not found.");
                context.AttachRange(exercise);

                //Handle many to many relationships manualy as that is the only way to avoid creating duplicates and to even delete a relationship

                if (updatedEntity.Description != "")
                    exercise.Description = updatedEntity.Description;

                if (updatedEntity.Equipment.Any())
                    exercise.Equipment = updatedEntity.Equipment;

                if (updatedEntity.PrimaryMuscleGroups.Any())
                    exercise.PrimaryMuscleGroups = updatedEntity.PrimaryMuscleGroups;

                if (updatedEntity.SecondaryMuscleGroups.Any())
                    exercise.SecondaryMuscleGroups = updatedEntity.SecondaryMuscleGroups;

                if (updatedEntity.PrimaryMuscles.Any())
                    exercise.PrimaryMuscles = updatedEntity.PrimaryMuscles;

                if (updatedEntity.SecondaryMuscles.Any())
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
