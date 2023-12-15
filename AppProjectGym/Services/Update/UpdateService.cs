using AppProjectGym.Utilities;
using System.Net.Http.Headers;
using System.Text.Json;
using AppInfo = AppProjectGym.Information.AppInfo;

namespace AppProjectGym.Services.Update
{
    public class UpdateService(HttpClient client) : IUpdateService
    {
        public async Task<bool> Update<T>(T updatedEntity, string endPoint = "") where T : class
        {
			try
			{
                if (endPoint == "") 
                    endPoint = typeof(T).Name;

                string url = AppInfo.BaseApiURL + "/" + endPoint;
                var stringContent = JsonSerializer.Serialize(updatedEntity);
                HttpRequestMessage message = new()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(url),
                    Content = new StringContent(stringContent)
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    }
                };

                var response = await client.SendAsync(message);
                response.EnsureSuccessStatusCode();
                return true;
			}
			catch (Exception ex)
			{
                LogDebugger.LogError(ex);
                return false;
			}
        }
    }
}
