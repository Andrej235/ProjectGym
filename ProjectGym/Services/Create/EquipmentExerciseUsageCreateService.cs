﻿using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Services.Create
{
    public class EquipmentExerciseUsageCreateService(ExerciseContext context, IReadService<EquipmentUsage> readService) : ICreateService<EquipmentUsage>
    {
        public async Task<object> Add(EquipmentUsage toAdd)
        {
            try
            {
                await readService.Get(x => x.EquipmentId == toAdd.EquipmentId && x.ExerciseId == toAdd.ExerciseId, "none");
                return default(int);
            }
            catch (NullReferenceException)
            {
                try
                {
                    await context.EquipmentUsages.AddAsync(toAdd);
                    await context.SaveChangesAsync();
                    return toAdd.Id;
                }
                catch (Exception ex)
                {
                    LogDebugger.LogError(ex);
                    return default(int);
                }
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return default(int);
            }
        }
    }
}
