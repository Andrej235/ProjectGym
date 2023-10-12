using AppProjectGym.Models;
using System.Diagnostics;
using System.Text.Json;

namespace AppProjectGym.Services
{
    public class ExerciseDataService : IDataService<Exercise>, IExerciseSearchData
    {
        private readonly JsonSerializerOptions serializerOptions;
        private readonly HttpClient httpClient;
        private readonly string baseApiURL;

        public ExerciseDataService()
        {
            serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            httpClient = new();
            baseApiURL = "http://192.168.1.9:5054/api";
        }

        public Task Create(Exercise newData) => throw new NotImplementedException();

        public Task Delete(int id) => throw new NotImplementedException();

        public async Task<List<Exercise>> Get()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseApiURL}/exercise?include=images");
                var body = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<AdvancedDTO<Exercise>>(body, serializerOptions).Values.OrderBy(e => e.Images == null || !e.Images.Any() ? int.MaxValue : e.Id).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
                return new();
            }
        }

        public async Task<Exercise> Get(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseApiURL}/exercise/{id}?include=all");
                var body = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<Exercise>(body, serializerOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
                return new();
            }
        }

        public async Task Get(OnDataLoad<Exercise> onDataLoad)
        {
            AdvancedDTO<Exercise> data = null;
            string startingUrl = "/exercise?include=all&offset=0&limit=10";
            do
            {
                var response = await httpClient.GetAsync(baseApiURL + (data == null ? startingUrl : data.NextBatchURLExtension));
                var body = await response.Content.ReadAsStringAsync();
                data = JsonSerializer.Deserialize<AdvancedDTO<Exercise>>(body, serializerOptions);
                onDataLoad(data.Values);
            } while (data.NextBatchURLExtension != null);
        }

        public async Task<List<Exercise>> Search(string q, string include = "none", int? offset = null, int? limit = null)
        {
            string limitQ = limit is not null ? $"&limit={limit}" : "";
            var response = await httpClient.GetAsync($"{baseApiURL}/exercise?offset={offset ?? 0}{limitQ}&include={include}&q={q}");
            var exercises = JsonSerializer.Deserialize<AdvancedDTO<Exercise>>(await response.Content.ReadAsStringAsync(), serializerOptions);

            return exercises.Values;
        }

        public Task Update(Exercise updatedData) => throw new NotImplementedException();

        public Task Update(int id, Exercise updatedData) => throw new NotImplementedException();
    }
}
