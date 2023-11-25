using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProjectGym.Data;
using ProjectGym.Models;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectGym.Controllers
{
    [ApiController]
    [Route("api/backup")]
    public class Backup(ExerciseContext context) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var tablesProperty = context
                .GetType()
                .GetProperties()
                .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            string resultingJSON = "{\n";
            resultingJSON += string.Join(", \n", tablesProperty.Select(property =>
            {
                if (property.GetValue(context) is IEnumerable<object> entities)
                {
                    string entitiesJSON = string.Join(", \n", entities.Select(Serialize));
                    string table = $"\"{property.Name}\": [\n{entitiesJSON}\n]";
                    return table;
                }

                return "";
            }));
            resultingJSON += "\n}";

            return Ok(resultingJSON);
        }

        [HttpPost]
        public async Task<IActionResult> LoadAsync()
        {
            StreamReader reader = new(Request.Body, Encoding.UTF8);
            string plainText = await reader.ReadToEndAsync();

            var tablesProperty = context
                 .GetType()
                 .GetProperties()
                 .Where(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            dynamic expandoObject = new ExpandoObject();

            Dictionary<string, object>? keyValues = JsonSerializer.Deserialize<Dictionary<string, object>>(plainText);
            if (keyValues is null)
                return Ok();

            foreach (var kvp in keyValues)
            {
                ((IDictionary<string, object>)expandoObject).Add(kvp);
            }

            return Ok(expandoObject.Exercises);
        }


        private string Serialize(object entity)
        {
            var properties = entity.GetType().GetProperties().Where(x => !x.PropertyType.IsGenericType);
            var resultingJSON = "{\n"
                + string.Join(", \n", properties.Select(property =>
                  {
                      string value = "";
                      if (property.PropertyType == typeof(string))
                          value = $"\"{property.GetValue(entity)}\"";
                      else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(bool))
                          value = $"{property.GetValue(entity)}".ToLower();
                      else
                          value = "\"Not implemented\"";

                      return $"\"{property.Name}\": {value}";
                  }))
                + "\n}";
            return resultingJSON;
        }

        public class StringDTO
        {
            public string Value { get; set; } = "";
        }
    }
}
