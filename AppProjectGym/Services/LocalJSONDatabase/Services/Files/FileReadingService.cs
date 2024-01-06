using System.Text.Json;
using LocalJSONDatabase.Services.Utility;

namespace LocalJSONDatabase.Services.Files
{
#nullable enable
    public class FileReadingService
    {
        public string FilePath { get; init; }
        private readonly FileStream file;

        public FileReadingService(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace.", nameof(filePath));

            FilePath = filePath;
            file = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
        }

        public string Read() => File.ReadAllText(FilePath);

        ~FileReadingService()
        {
            file.Close();
            file.Dispose();
        }
    }
}
