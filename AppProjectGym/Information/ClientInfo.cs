﻿using AppProjectGym.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

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

        public static async Task<bool> Login(string email, string password)
        {
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
                return user is not null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception occured: {ex}");
                return false;
            }
        }

        public static async Task<bool> SetUser()
        {
            if (ClientGuid is null)
                return false;

            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync($"{AppInfo.BaseApiURL}/user/client/{ClientGuid}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                User user = JsonSerializer.Deserialize<User>(content, AppInfo.DeserializationOptions);

                User = user;
                return User is not null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"---> Exception occured: {ex}");
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
