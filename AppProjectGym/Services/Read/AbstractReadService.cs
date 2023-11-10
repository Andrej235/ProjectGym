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
    public abstract class AbstractReadService<T> : IReadService<T> where T : class
    {
        private readonly HttpClient client;
        protected AbstractReadService(HttpClient client)
        {
            this.client = client;
        }

        protected abstract string URLExtension { get; }

        public virtual async Task<T> Get(string primaryKey, string include = "all")
        {
            try
            {
                var response = await client.GetAsync($"{AppInfo.BaseApiURL}/{URLExtension}/{primaryKey}?include={include}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var exercise = JsonSerializer.Deserialize<T>(content, AppInfo.DeserializationOptions);
                return exercise;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message}");
                throw;
            }
        }

        public virtual async Task<List<T>> Get(string query, int? offset = 0, int? limit = -1, string include = "all")
        {
            try
            {
                var url = $"{AppInfo.BaseApiURL}/{URLExtension}?include={include}&q={query}&offset={offset}&limit={limit}";
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                var exercises = JsonSerializer.Deserialize<List<T>>(content, AppInfo.DeserializationOptions);
                return exercises;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message}");
                throw;
            }
        }
    }
}