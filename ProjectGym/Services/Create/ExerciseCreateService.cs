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
        private readonly ICreateService<EquipmentExerciseUsage, int> equipmentExerciseUsageCreateService;
        private readonly ICreateService<PrimaryMuscleExerciseConnection, int> primaryMuscleExerciseConnectionCreateService;
        private readonly ICreateService<SecondaryMuscleExerciseConnection, int> secondaryMuscleExerciseConnectionCreateService;

        public ExerciseCreateService(ExerciseContext context, IReadService<Exercise> readService, ICreateService<EquipmentExerciseUsage, int> equipmentExerciseUsageCreateService, ICreateService<PrimaryMuscleExerciseConnection, int> primaryMuscleExerciseConnectionCreateService, ICreateService<SecondaryMuscleExerciseConnection, int> secondaryMuscleExerciseConnectionCreateService)
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
