using AppProjectGym.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace AppProjectGym.Services
{
    public class ExerciseDataService : IDataService<Exercise>
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
                var response = await httpClient.GetAsync("http://192.168.1.9:5054/api/exercise/basic");
                var body = await response.Content.ReadAsStringAsync();

                var res = JsonSerializer.Deserialize<List<Exercise>>(body, serializerOptions);
                res = res.OrderBy(e => e.Images == null || !e.Images.Any() ? int.MaxValue : e.Id).ToList();

                _ = Get(e => Debug.WriteLine(e.Name));

                return res;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
                return new();
            }
        }

        public Task<Exercise> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Get(OnDataLoad<Exercise> onDataLoad)
        {
            AdvancedDTO<Exercise> data = null;
            string startingUrl = "/exercise/querytest?include=all&offset=0&limit=10";
            do
            {
                var response = await httpClient.GetAsync(baseApiURL + (data == null ? startingUrl : data.NextBatchURLExtension));
                var body = await response.Content.ReadAsStringAsync();
                data = JsonSerializer.Deserialize<AdvancedDTO<Exercise>>(body, serializerOptions);
                foreach (var e in data.Values)
                {
                    onDataLoad(e);
                }
            } while (data.NextBatchURLExtension != null);
        }

        public Task Update(Exercise updatedData) => throw new NotImplementedException();

        public Task Update(int id, Exercise updatedData) => throw new NotImplementedException();
    }
}
