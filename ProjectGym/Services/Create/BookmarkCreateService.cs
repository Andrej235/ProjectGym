using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
{
    public class BookmarkCreateService(ExerciseContext context, IReadService<ExerciseBookmark> bookmarkReadService) : CreateService<ExerciseBookmark>(context)
    {
        protected async override Task<Exception?> IsEntityValid(ExerciseBookmark entity)
        {
            try
            {
                await bookmarkReadService.Get(x => x.UserId == entity.UserId && x.ExerciseId == entity.ExerciseId, "none");
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
