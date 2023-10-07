using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppProjectGym.Models;

namespace AppProjectGym.Services
{
    public class MuscleDataService : IDataService<Muscle>
    {
        private readonly JsonSerializerOptions serializerOptions;
        private readonly HttpClient httpClient;
        private readonly string baseApiURL;

        public MuscleDataService()
        {
            serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            httpClient = new();
            baseApiURL = "http://192.168.1.9:5054/api";
        }

        public Task Create(Muscle newData)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Muscle>> Get()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseApiURL}/muscle");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Muscle>>(content, serializerOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
                return null;
            }
        }

        public async Task<Muscle> Get(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseApiURL}/muscle/{id}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Muscle>(content, serializerOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
                return null;
            }
        }

        public async Task Get(OnDataLoad<Muscle> onDataLoad) => onDataLoad(await Get());

        public Task Update(Muscle updatedData)
        {
            throw new NotImplementedException();
        }

        public Task Update(int id, Muscle updatedData)
        {
            throw new NotImplementedException();
        }
    }
}
