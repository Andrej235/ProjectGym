using System.Text.Json;

namespace AppProjectGym.Utilities
{
    public static class CollectionExtensions
    {
        public static List<T> DeepCopy<T>(this List<T> originalList) => JsonSerializer.Deserialize<List<T>>(JsonSerializer.Serialize(originalList));
    }
}
