using System.Diagnostics;
using System.Text.Json;
using AppInfo = AppProjectGym.Information.AppInfo;

namespace AppProjectGym.Services.Read
{
    public abstract class AbstractReadService<T> : IReadService<T> where T : class
    {
        private readonly HttpClient client;
        /// <summary>
        /// Initializes a new instance of the AbstractReadService class with an HttpClient.
        /// </summary>
        /// <param name="client">The HttpClient instance used for making HTTP requests.</param>
        protected AbstractReadService(HttpClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Gets the URL extension for the specific service.
        /// </summary>
        protected abstract string URLExtension { get; }

        ///<inheritdoc cref="IReadService{T}.Get(string, string)"/>
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

        ///<inheritdoc cref="IReadService{T}.Get(string, int?, int?, string)"/>
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