using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using AppInfo = AppProjectGym.Information.AppInfo;

namespace AppProjectGym.Services.Create
{
    public abstract class AbstractCreateService<T> : ICreateService<T> where T : class
    {
        private readonly HttpClient client;
        /// <summary>
        /// Initializes a new instance of the AbstractCreateService class with an HttpClient.
        /// </summary>
        /// <param name="client">The HttpClient instance used for making HTTP requests.</param>
        public AbstractCreateService(HttpClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Gets the URL extension for the specific service.
        /// </summary>
        protected abstract string URLExtension { get; }

        ///<inheritdoc cref="ICreateService{T}.Add(T)"/>
        public async Task<bool> Add(T entityToAdd)
        {
            try
            {
                var toAddJSON = JsonSerializer.Serialize(entityToAdd, AppInfo.SerializerOptions);
                HttpRequestMessage request = new()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{AppInfo.BaseApiURL}/{URLExtension}"),
                    Content = new StringContent(toAddJSON)
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    }
                };

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
