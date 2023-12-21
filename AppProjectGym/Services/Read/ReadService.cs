using AppProjectGym.Utilities;
using System.Text.Json;
using AppInfo = AppProjectGym.Information.AppInfo;

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

                string url = $"{AppInfo.BaseApiURL}/{endPoint}{(!endPoint.Contains('?') ? "?" : "&")}include={include}&q={string.Join(";", query)}";

                HttpRequestMessage request = new()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url)
                };

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                T result = default;
                try
                {
                    result = JsonSerializer.Deserialize<T>(content, AppInfo.DeserializationOptions);
                }
                catch (Exception)
                {
                    try
                    {
                        if (!typeof(T).IsGenericType || typeof(T).GetGenericTypeDefinition() != typeof(IEnumerable<>))
                            result = JsonSerializer.Deserialize<IEnumerable<T>>(content, AppInfo.DeserializationOptions).First();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return null;
            }
        }

        public static string TranslateEndPoint(string endPoint, int? offset, int? limit) => $"{endPoint}?offset={offset ?? 0}&limit={limit ?? -1}";
    }
}