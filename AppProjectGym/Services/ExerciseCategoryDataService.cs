﻿using AppProjectGym.Models;
using System.Diagnostics;
using System.Text.Json;

namespace AppProjectGym.Services
{
    public class ExerciseCategoryDataService : IDataService<ExerciseCategory>
    {
        private readonly JsonSerializerOptions serializerOptions;
        private readonly HttpClient httpClient;
        private readonly string baseApiURL;

        public ExerciseCategoryDataService()
        {
            serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            httpClient = new();
            baseApiURL = "http://192.168.1.9:5054/api";
        }

        public Task Create(ExerciseCategory newData)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ExerciseCategory>> Get()
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseApiURL}/category");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ExerciseCategory>>(content, serializerOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
                return null;
            }
        }

        public async Task<ExerciseCategory> Get(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"{baseApiURL}/category/{id}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ExerciseCategory>(content, serializerOptions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception: {ex}");
                return null;
            }
        }

        public async Task Get(OnDataLoad<ExerciseCategory> onDataLoad) => onDataLoad(await Get());

        public Task Update(ExerciseCategory updatedData)
        {
            throw new NotImplementedException();
        }

        public Task Update(int id, ExerciseCategory updatedData)
        {
            throw new NotImplementedException();
        }
    }
}
