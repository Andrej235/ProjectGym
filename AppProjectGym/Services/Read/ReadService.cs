using AppProjectGym.Utilities;
using AppInfo = AppProjectGym.Information.AppInfo;
using System.Text.Json;

namespace AppProjectGym.Services.Read
{
    public class ReadService<T>(HttpClient client) : IEntityReadService<T> where T : class
    {
        public async Task<T> Get(string endPoint, string include = "all", params string[] query)
        {
            try
            {
                string url = AppInfo.BaseApiURL + endPoint + include + string.Join(";", query);
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content);
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                throw;
            }
        }
    }
}
