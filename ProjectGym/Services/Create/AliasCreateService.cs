﻿using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class AliasCreateService(ExerciseContext context, IReadService<Exercise> exerciseReadService) : ICreateService<Alias, int>
    {
        public async Task<int> Add(Alias toAdd)
        {
            if (toAdd.ExerciseId < 1)
                return default;

            try
            {
                await exerciseReadService.Get(x => x.Id == toAdd.ExerciseId, "none");

                await context.Aliases.AddAsync(toAdd);
                await context.SaveChangesAsync();

                return toAdd.Id;
            }
            catch (NullReferenceException)
            {

                Debug.WriteLine($"Exercise with Id {toAdd.ExerciseId} was not found");
                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error occured while trying to add Image: {ex.Message}");
                return default;
            }
        }

    }
}
