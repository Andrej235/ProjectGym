using AppProjectGym.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AppProjectGym.Information;
using AppInfo = AppProjectGym.Information.AppInfo;
using Image = AppProjectGym.Models.Image;

namespace AppProjectGym.Services.Read
{
    public class ImageReadService : IReadService<Image>
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public ImageReadService()
        {
            client = new();
            jsonSerializerOptions = new()
            {
                PropertyNameCaseInsensitive = true,
            };
        }

        public async Task<Image> Get(string primaryKey, string include = "all")
        {
            if (int.TryParse(primaryKey, out int id))
            {
                try
                {
                    var response = await client.GetAsync($"{AppInfo.BaseApiURL}/image/{id}?include={include}");
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var image = JsonSerializer.Deserialize<Image>(content, jsonSerializerOptions);
                    return image;
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

        public Task<List<Image>> Get(string query, int? offset = 0, int? limit = -1, string include = "all")
        {
            throw new NotImplementedException();
        }
    }
}
