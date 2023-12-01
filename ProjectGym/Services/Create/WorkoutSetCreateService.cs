using ProjectGym.Data;
using ProjectGym.Models;

namespace ProjectGym.Services.Create
{
    public class WorkoutSetCreateService(ExerciseContext context) : CreateService<WorkoutSet>(context) { }
}