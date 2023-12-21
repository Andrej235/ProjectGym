using System.Text.Json;

namespace AppProjectGym.Utilities
{
    public static class CollectionExtensions
    {
        public static List<T> DeepCopy<T>(this List<T> originalList) => JsonSerializer.Deserialize<List<T>>(JsonSerializer.Serialize(originalList));
        public static T DeepCopy<T>(this T originalEntity) => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(originalEntity));
    }
}
