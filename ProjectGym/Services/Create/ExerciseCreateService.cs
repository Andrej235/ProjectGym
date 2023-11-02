using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class ExerciseCreateService : ICreateService<Exercise>
    {
        private readonly ExerciseContext context;
        private readonly IReadService<Exercise> readService;
        private readonly ICreateService<EquipmentExerciseUsage> equipmentExerciseUsageCreateService;
        private readonly ICreateService<PrimaryMuscleExerciseConnection> primaryMuscleExerciseConnectionCreateService;
        private readonly ICreateService<SecondaryMuscleExerciseConnection> secondaryMuscleExerciseConnectionCreateService;

        public ExerciseCreateService(ExerciseContext context, IReadService<Exercise> readService, ICreateService<EquipmentExerciseUsage> equipmentExerciseUsageCreateService, ICreateService<PrimaryMuscleExerciseConnection> primaryMuscleExerciseConnectionCreateService, ICreateService<SecondaryMuscleExerciseConnection> secondaryMuscleExerciseConnectionCreateService)
        {
            this.context = context;
            this.readService = readService;
            this.equipmentExerciseUsageCreateService = equipmentExerciseUsageCreateService;
            this.primaryMuscleExerciseConnectionCreateService = primaryMuscleExerciseConnectionCreateService;
            this.secondaryMuscleExerciseConnectionCreateService = secondaryMuscleExerciseConnectionCreateService;
        }

        public async Task<bool> Add(Exercise toAdd)
        {
            try
            {
                await readService.Get(x => x.Name.ToLower() == toAdd.Name.ToLower(), "none");
                return false;
            }
            catch (NullReferenceException)
            {
                try
                {
                    context.AttachRange(toAdd.Category);
                    context.AttachRange(toAdd.Notes);
                    context.AttachRange(toAdd.Images);
                    context.AttachRange(toAdd.Videos);

                    Exercise dbEntity = new()
                    {
                        UUID = toAdd.UUID,
                        Name = toAdd.Name,
                        Description = toAdd.Description,
                        Notes = toAdd.Notes,
                        Images = toAdd.Images,
                        Videos = toAdd.Videos,
                        Category = toAdd.Category
                    };

                    await context.Exercises.AddAsync(dbEntity);
                    await context.SaveChangesAsync();

                    foreach (var e in toAdd.Equipment)
                        await equipmentExerciseUsageCreateService.Add(new() { EquipmentId = e.Id, ExerciseId = dbEntity.Id });

                    foreach (var m in toAdd.PrimaryMuscles)
                        await primaryMuscleExerciseConnectionCreateService.Add(new() { MuscleId = m.Id, ExerciseId = dbEntity.Id });

                    foreach (var m in toAdd.SecondaryMuscles)
                        await secondaryMuscleExerciseConnectionCreateService.Add(new() { MuscleId = m.Id, ExerciseId = dbEntity.Id });

                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message} \n{ex.InnerException?.Message}");
                return false;
            }
        }
    }
}
