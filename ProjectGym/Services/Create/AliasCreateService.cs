﻿using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class AliasCreateService : ICreateService<ExerciseAlias, int>
    {
        private readonly ExerciseContext context;
        private readonly IReadService<Exercise> exerciseReadService;
        public AliasCreateService(ExerciseContext context, IReadService<Exercise> exerciseReadService)
        {
            this.context = context;
            this.exerciseReadService = exerciseReadService;
        }

        public async Task<int> Add(ExerciseAlias toAdd)
        {
            if (toAdd.ExerciseId < 1)
                return default;

            try
            {
                await exerciseReadService.Get(x => x.Id == toAdd.ExerciseId, "none");

                await context.ExerciseAliases.AddAsync(toAdd);
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
