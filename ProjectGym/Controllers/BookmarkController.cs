using Microsoft.AspNetCore.Mvc;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookmarkController(IReadService<ExerciseBookmark> readService, ICreateService<ExerciseBookmark> createService, IDeleteService<ExerciseBookmark> deleteService) : ControllerBase
    {
        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromBody] ExerciseBookmark entityToAdd)
        {
            try
            {
                var bookmark = await readService.Get(x => x.UserId == entityToAdd.UserId && x.ExerciseId == entityToAdd.ExerciseId, "none"); //Throws NullReferenceException if it doesn't exist
                await deleteService.Delete(bookmark.Id);
                return Ok(false);
            }
            catch (NullReferenceException)
            {
                await createService.Add(entityToAdd);
                return Ok(true);
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }
    }
}
