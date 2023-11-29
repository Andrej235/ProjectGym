using AppProjectGym.Utilities;
using AppInfo = AppProjectGym.Information.AppInfo;
using System.Text.Json;

namespace AppProjectGym.Services.Read
{
    public class ReadService(HttpClient client) : IReadService
    {
        public async Task<T> Get<T>(string include = "none", string endPoint = "", params string[] query) where T : class
        {
            try
            {
                if (endPoint == "")
                {
                    Type type = typeof(T);
                    endPoint = type.IsGenericType ? type.GetGenericArguments()[0].Name : type.Name;
                }

                string url = $"{AppInfo.BaseApiURL}/{endPoint}";
                url += !endPoint.Contains('?') ? "?" : "&";
                url += $"include={include}&q={string.Join(";", query)}";

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                T result = JsonSerializer.Deserialize<T>(content, AppInfo.DeserializationOptions);
                return result;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return null;
            }
        }

        public static string TranslateEndPoint(string endPoint, int? offset, int? limit) => $"{endPoint}?{offset ?? 0}&{limit ?? -1}";
    }
}