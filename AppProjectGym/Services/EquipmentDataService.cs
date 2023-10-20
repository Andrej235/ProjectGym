using AppProjectGym.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppProjectGym.Services
{
    public class EquipmentDataService : IDataService<Equipment>
    {
        private readonly JsonSerializerOptions serializerOptions;
        private readonly HttpClient httpClient;
        private readonly string baseApiURL;

        public EquipmentDataService()
        {
            serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            httpClient = new();
            baseApiURL = "http://192.168.1.9:5054/api";
        }

        public Task Create(Equipment newData)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Equipment>> Get()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseApiURL}/equipment");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Equipment>>(content, serializerOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
                return null;
            }
        }

        public async Task<Equipment> Get(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseApiURL}/equipment/{id}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Equipment>(content, serializerOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
                return null;
            }
        }

        public async Task Get(OnDataLoad<Equipment> onDataLoad) => onDataLoad(await Get());

        public Task Update(Equipment updatedData)
        {
            throw new NotImplementedException();
        }

        public Task Update(int id, Equipment updatedData)
        {
            throw new NotImplementedException();
        }
    }
}
