using ProjectGym.Data;
using ProjectGym.Models;

namespace ProjectGym.Services.Create
{
    public class WeightCreateService : ICreateService<UserExerciseWeight, Guid>
    {
/*        private readonly ExerciseContext context;
        public WeightCreateService(ExerciseContext context)
        {
            this.context = context;
        }*/

        public /*async*/ Task<Guid> Add(UserExerciseWeight toAdd)
        {
/*            if (toAdd.UserId == default || toAdd.ExerciseId == default)
                return default;*/

            throw new NotImplementedException();
        }
    }
}
