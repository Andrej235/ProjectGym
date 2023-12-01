using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppProjectGym.Information
{
    public static class AppInfo
    {
        public static string BaseApiURL => "http://192.168.1.100:5054/api";

        public static JsonSerializerOptions DeserializationOptions => deserializationOptions;
        private static readonly JsonSerializerOptions deserializationOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static JsonSerializerOptions SerializationOptions => serializationOptions;
        private static readonly JsonSerializerOptions serializationOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }
}
