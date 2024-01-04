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
        public async Task<IActionResult> Create([FromBody] ExerciseBookmark entityToAdd)
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

        [HttpGet]
        public virtual async Task<IActionResult> Get([FromQuery] int? exerciseId, [FromQuery] Guid? userId)
        {
            try
            {
                if (exerciseId is null || userId is null)
                    return BadRequest("exerciseId and userId MUST be entered in the query");

                var bookmark = await readService.Get(x => x.UserId == userId && x.ExerciseId == exerciseId, "none");
                return Ok(bookmark);
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpGet("byuser/{userId}")]
        public virtual async Task<IActionResult> Get(Guid userId)
        {
            try
            {
                if (userId == default)
                    return BadRequest("Invalid user id");

                var bookmarks = await readService.Get(x => x.UserId == userId, 0, -1, "none");
                return Ok(bookmarks);
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }
    }
}
