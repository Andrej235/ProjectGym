using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class ExerciseCreateService : ICreateService<Exercise, int>
    {
        private readonly ExerciseContext context;
        private readonly IReadService<Exercise> readService;
        private readonly ICreateService<EquipmentUsage, int> equipmentExerciseUsageCreateService;
        private readonly ICreateService<PrimaryMuscleGroupInExercise, int> primaryMuscleExerciseConnectionCreateService;
        private readonly ICreateService<SecondaryMuscleGroupInExercise, int> secondaryMuscleExerciseConnectionCreateService;

        public ExerciseCreateService(ExerciseContext context, IReadService<Exercise> readService, ICreateService<EquipmentUsage, int> equipmentExerciseUsageCreateService, ICreateService<PrimaryMuscleGroupInExercise, int> primaryMuscleExerciseConnectionCreateService, ICreateService<SecondaryMuscleGroupInExercise, int> secondaryMuscleExerciseConnectionCreateService)
        {
            this.context = context;
            this.readService = readService;
            this.equipmentExerciseUsageCreateService = equipmentExerciseUsageCreateService;
            this.primaryMuscleExerciseConnectionCreateService = primaryMuscleExerciseConnectionCreateService;
            this.secondaryMuscleExerciseConnectionCreateService = secondaryMuscleExerciseConnectionCreateService;
        }

        public async Task<int> Add(Exercise toAdd)
        {
            try
            {
                await readService.Get(x => x.Name.ToLower() == toAdd.Name.ToLower(), "none");
                return default;
            }
            catch (NullReferenceException)
            {
                try
                {
                    context.AttachRange(toAdd.Notes);
                    context.AttachRange(toAdd.Images);

                    Exercise dbEntity = new()
                    {
                        Name = toAdd.Name,
                        Description = toAdd.Description,
                        Notes = toAdd.Notes,
                        Images = toAdd.Images,
                    };

                    await context.Exercises.AddAsync(dbEntity);
                    await context.SaveChangesAsync();

                    foreach (var e in toAdd.Equipment)
                        await equipmentExerciseUsageCreateService.Add(new() { EquipmentId = e.Id, ExerciseId = dbEntity.Id });

                    foreach (var m in toAdd.PrimaryMuscleGroups)
                        await primaryMuscleExerciseConnectionCreateService.Add(new() { MuscleGroupId = m.Id, ExerciseId = dbEntity.Id });

                    foreach (var m in toAdd.SecondaryMuscleGroups)
                        await secondaryMuscleExerciseConnectionCreateService.Add(new() { MuscleGroupId = m.Id, ExerciseId = dbEntity.Id });

                    await context.SaveChangesAsync();
                    return dbEntity.Id;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                return default;
            }
        }
    }
}
