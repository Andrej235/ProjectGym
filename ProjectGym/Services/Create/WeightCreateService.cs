using ProjectGym.Data;
using ProjectGym.Models;

namespace ProjectGym.Services.Create
{
    public class WeightCreateService : ICreateService<PersonalExerciseWeight, Guid>
    {
/*        private readonly ExerciseContext context;
        public WeightCreateService(ExerciseContext context)
        {
            this.context = context;
        }*/

        public /*async*/ Task<Guid> Add(PersonalExerciseWeight toAdd)
        {
/*            if (toAdd.UserId == default || toAdd.ExerciseId == default)
                return default;*/

            throw new NotImplementedException();
        }
    }
}
