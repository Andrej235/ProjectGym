using AppProjectGym.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using AppProjectGym.Utilities;

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
            IsLoadingData = true;
            var client = new HttpClient();
            var loginInfo = JsonSerializer.Serialize(new LoginDTO()
            {
                ClientGuid = ClientGuid,
                Email = email,
                Password = password,
            }, AppInfo.SerializationOptions);

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



        private class LoginDTO
        {
            public Guid? ClientGuid { get; set; }
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        private class LoggedInDTO
        {
            public Guid ClientGuid { get; set; }
            public User User { get; set; }
        }
    }
}
