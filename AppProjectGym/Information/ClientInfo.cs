using AppProjectGym.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using AppProjectGym.Utilities;
using System.Xml.Linq;

namespace AppProjectGym.Information
{
    public static class ClientInfo
    {
        private static User user;
        public static User User
        {
            get => user;
            private set => user = value;
        }

        private static Guid? clientGuid;
        public static Guid? ClientGuid
        {
            get
            {
                clientGuid ??= Preferences.ContainsKey("ClientGuid") ? Guid.Parse(Preferences.Get("ClientGuid", null)) : null;
                return clientGuid;
            }
            private set
            {
                clientGuid = value;
                if (value == null || value == Guid.Empty)
                    Preferences.Remove("ClientGuid");
                else
                    Preferences.Set("ClientGuid", value.ToString());
            }
        }

        public static bool IsLoadingData { get; private set; }

        public static async Task<bool> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
                return false;

            IsLoadingData = true;
            var client = new HttpClient();
            var loginInfo = JsonSerializer.Serialize(new LoginDTO(ClientGuid, email, password), AppInfo.SerializationOptions);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{AppInfo.BaseApiURL}/user/login"),
                Content = new StringContent(loginInfo)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };

            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var userClient = JsonSerializer.Deserialize<LoggedInDTO>(content, AppInfo.DeserializationOptions);

                ClientGuid = userClient.ClientGuid;
                User = userClient.User;
                IsLoadingData = false;
                return User != null;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                IsLoadingData = false;
                return false;
            }
        }

        public static async Task<bool> Register(string name, string email, string password)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
                return false;

            IsLoadingData = true;
            var client = new HttpClient();
            var loginInfo = JsonSerializer.Serialize(new RegisterDTO(ClientGuid, name, email, password), AppInfo.SerializationOptions);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{AppInfo.BaseApiURL}/user"),
                Content = new StringContent(loginInfo)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };

            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var userClient = JsonSerializer.Deserialize<LoggedInDTO>(content, AppInfo.DeserializationOptions);

                ClientGuid = userClient.ClientGuid;
                User = userClient.User;
                IsLoadingData = false;
                return User != null;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                IsLoadingData = false;
                return false;
            }
        }

        public static async Task<bool> Logout()
        {
            IsLoadingData = true;
            var client = new HttpClient();
            var url = $"{AppInfo.BaseApiURL}/user/logout/{ClientGuid}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(url),
            };

            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                IsLoadingData = false;
                User = null;
                return true;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                IsLoadingData = false;
                return false;
            }
        }

        public static async Task<bool> SetUser()
        {
            if (ClientGuid is null)
                return false;

            IsLoadingData = true;
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync($"{AppInfo.BaseApiURL}/user/client/{ClientGuid}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                User user = JsonSerializer.Deserialize<User>(content, AppInfo.DeserializationOptions);

                User = user;
                IsLoadingData = false;
                return User != null;
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                IsLoadingData = false;
                return false;
            }
        }



        private record LoginDTO(Guid? ClientGuid, string Email, string Password);

        private record LoggedInDTO(Guid ClientGuid, User User);

        private record RegisterDTO(Guid? ClientGuid, string Name, string Email, string Password);
    }
}
