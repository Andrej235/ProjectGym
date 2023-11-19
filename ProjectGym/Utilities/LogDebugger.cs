using System.Diagnostics;

namespace ProjectGym.Utilities
{
    public static class LogDebugger
    {
        public static void LogError(Exception ex)
        {
            if (ex.InnerException is null)
                Debug.WriteLine($"---> Error occurred: {ex.Message}");
            else
                Debug.WriteLine($"---> Error occurred: {ex.Message}\n{ex.InnerException.Message}");
        }

        public static string GetErrorMessage(Exception ex)
        {
            return ex.InnerException is null
                ? $"---> Error occurred: {ex.Message}"
                : $"---> Error occurred: {ex.Message}\n{ex.InnerException.Message}";
        }
    }
}
