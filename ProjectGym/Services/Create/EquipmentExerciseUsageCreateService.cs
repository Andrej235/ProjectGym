﻿using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class EquipmentExerciseUsageCreateService : ICreateService<EquipmentExerciseUsage>
    {
        private readonly ExerciseContext context;
        private readonly IReadService<EquipmentExerciseUsage> readService;
        public EquipmentExerciseUsageCreateService(ExerciseContext context, IReadService<EquipmentExerciseUsage> readService)
        {
            this.context = context;
            this.readService = readService;
        }

        public async Task<bool> Add(EquipmentExerciseUsage toAdd)
        {
            try
            {
                await readService.Get(x => x.EquipmentId == toAdd.EquipmentId && x.ExerciseId == toAdd.ExerciseId, "none");
                return false;
            }
            catch (NullReferenceException)
            {
                try
                {
                    await context.EquipmentExerciseUsages.AddAsync(toAdd);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error occured while trying to add EquipmentExerciseUsage: {ex.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while trying to add EquipmentExerciseUsage: {ex.Message}");
                return false;
            }
        }
    }
}
