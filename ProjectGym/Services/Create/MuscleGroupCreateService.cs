using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using System.Diagnostics;

namespace ProjectGym.Services.Create
{
    public class MuscleGroupCreateService(ExerciseContext context, IReadService<MuscleGroup> readService) : CreateService<MuscleGroup>(context)
    {
        protected override async Task<Exception?> IsEntityValid(MuscleGroup entity)
        {
            try
            {
                await readService.Get(x => x.Name.ToLower().Trim() == entity.Name.ToLower().Trim(), "none");
                return new Exception("Entity already exists");
            }
            catch (NullReferenceException)
            {
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
