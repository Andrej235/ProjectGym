using AppProjectGym.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppInfo = AppProjectGym.Information.AppInfo;

namespace AppProjectGym.Services
{
    public class NotesDataService : IDataService<ExerciseNote>
    {
        private readonly JsonSerializerOptions serializerOptions;
        private readonly HttpClient httpClient;

        public NotesDataService()
        {
            serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            httpClient = new();
        }



        public Task Create(ExerciseNote newData)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ExerciseNote>> Get()
        {
            try
            {
                var response = await httpClient.GetAsync($"{AppInfo.BaseApiURL}/note");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ExerciseNote>>(content, serializerOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
                return null;
            }
        }

        public async Task<ExerciseNote> Get(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{AppInfo.BaseApiURL}/note/{id}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ExerciseNote>(content, serializerOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
                return null;
            }
        }

        public Task Get(OnDataLoad<ExerciseNote> onDataLoad)
        {
            throw new NotImplementedException();
        }

        public Task Update(ExerciseNote updatedData)
        {
            throw new NotImplementedException();
        }

        public Task Update(int id, ExerciseNote updatedData)
        {
            throw new NotImplementedException();
        }
    }
}
