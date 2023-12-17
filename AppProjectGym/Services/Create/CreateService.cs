using AppProjectGym.Utilities;
using System.Net.Http.Headers;
using System.Text.Json;
using AppInfo = AppProjectGym.Information.AppInfo;

namespace AppProjectGym.Services.Create
{
    public class CreateService(HttpClient client) : ICreateService
    {
        public async Task<string> Add<T>(T entityToAdd, string endPoint = "") where T : class
        {
            try
            {
                if (endPoint == "")
                    endPoint = typeof(T).Name;

                var toAddJSON = JsonSerializer.Serialize(entityToAdd, AppInfo.SerializationOptions);
                HttpRequestMessage request = new()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{AppInfo.BaseApiURL}/{endPoint}"),
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
                return content.Replace("\"", "");
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return default;
            }
        }
    }
}
