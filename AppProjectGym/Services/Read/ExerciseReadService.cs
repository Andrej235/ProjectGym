using AppProjectGym.Information;
using AppProjectGym.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppInfo = AppProjectGym.Information.AppInfo;

namespace AppProjectGym.Services.Read
{
    public class ExerciseReadService : IReadService<Exercise>
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions jsonSerializerOptions;
        public ExerciseReadService()
        {
            client = new HttpClient();
            jsonSerializerOptions = new()
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<Exercise> Get(string primaryKey, string include = "all")
        {
            if (int.TryParse(primaryKey, out int id))
            {
                try
                {
                    var response = await client.GetAsync($"{AppInfo.BaseApiURL}/exercise/{id}?include={include}");
                    response.EnsureSuccessStatusCode();
                    
                    var content = await response.Content.ReadAsStringAsync();
                    var exercise = JsonSerializer.Deserialize<Exercise>(content, jsonSerializerOptions);
                    return exercise;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"---> Error occurred: {ex.Message}");
                    throw;
                }
            }
            else
            {
                throw new NotSupportedException("Primary key of exercises MUST be an integer");
            }
        }

        public async Task<List<Exercise>> Get(string query, int? offset = 0, int? limit = -1, string include = "all")
        {
            try
            {
                var response = await client.GetAsync($"{AppInfo.BaseApiURL}/exercise?include={include}&q={query}&offset={offset}&limit={limit}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var exercises = JsonSerializer.Deserialize<AdvancedDTO<Exercise>>(content, jsonSerializerOptions);
                return exercises.Values;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
