using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using AppInfo = AppProjectGym.Information.AppInfo;

namespace AppProjectGym.Services.Create
{
    public abstract class AbstractCreateService<TEntity, TPK> : ICreateService<TEntity, TPK> where TEntity : class
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

        protected abstract Func<string, TPK> ParsePrimaryKey { get; }

        ///<inheritdoc cref="ICreateService{TEntity ,TPK}.Add(TEntity)"/>
        public async Task<TPK> Add(TEntity entityToAdd)
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
                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    var newEntityId = ParsePrimaryKey(content);
                    return newEntityId;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"---> Error occurred while parsing primary key: {ex.Message}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Error occurred: {ex.Message}");
                return default;
            }
        }
    }
}
