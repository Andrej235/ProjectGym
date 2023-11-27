using Microsoft.AspNetCore.Mvc;
using ProjectGym.Data;
using ProjectGym.Utilities;
using System.Text;

namespace ProjectGym.Controllers
{
    [ApiController]
    [Route("api/database")]
    public class DatabaseController(ExerciseContext context) : ControllerBase
    {
        struct OldNewIdPairs(object oldId, object newId)
        {
            public object oldId = oldId;
            public object newId = newId;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(DatabaseSerializationService.Serialize(context));
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Load()
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                StreamReader reader = new(Request.Body, Encoding.UTF8);
                await DatabaseDeserializationService.LoadDatabaseAsync(context, await reader.ReadToEndAsync(), new KeyValuePair<string, string>("Creator", "User"));

                return Ok();
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDB()
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDB()
        {
            try
            {
                await context.Database.EnsureDeletedAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        public class StringDTO
        {
            public string Value { get; set; } = "";
        }
    }
}