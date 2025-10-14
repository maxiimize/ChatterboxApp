using ChatterboxApp.Models;
using System.Text.Json;

namespace ChatterboxApp.Services
{
    public class ChatHistoryService
    {
        private readonly ChatSession _currentSession;
        private readonly string _chatFilesDirectory;

        public ChatHistoryService()
        {
            _currentSession = new ChatSession();
            _chatFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "ChatFiles");
            EnsureDirectoryExists();
        }

        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(_chatFilesDirectory))
            {
                Directory.CreateDirectory(_chatFilesDirectory);
            }
        }

        public ChatSession GetCurrentSession()
        {
            return _currentSession;
        }

        public void AddMessage(string role, string content)
        {
            var message = new ChatMessage(role, content);
            if (message.IsValid())
            {
                _currentSession.AddMessage(message);
            }
        }

        public List<ChatMessage> GetMessagesDescending()
        {
            return _currentSession.GetMessagesDescending();
        }

        public List<ChatMessage> GetMessagesAscending()
        {
            return _currentSession.Messages.OrderBy(m => m.Timestamp).ToList();
        }

        public void SaveAllChatsToFile()
        {
            try
            {
                if (_currentSession.GetMessageCount() == 0)
                {
                    return;
                }

                var fileName = GenerateFileName();
                var filePath = Path.Combine(_chatFilesDirectory, fileName);
                var jsonContent = SerializeSession();

                File.WriteAllText(filePath, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid sparande av chat: {ex.Message}");
            }
        }

        private string GenerateFileName()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var serialNumber = GetNextSerialNumber(today);

            return $"{today}_chattnr_{serialNumber:D2}.json";
        }

        private int GetNextSerialNumber(string date)
        {
            try
            {
                var existingFiles = Directory.GetFiles(_chatFilesDirectory, $"{date}_chattnr_*.json");

                if (existingFiles.Length == 0)
                {
                    return 1;
                }

                var maxNumber = 0;
                foreach (var file in existingFiles)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);
                    var parts = fileName.Split('_');

                    if (parts.Length >= 3 && int.TryParse(parts[2], out int number))
                    {
                        if (number > maxNumber)
                        {
                            maxNumber = number;
                        }
                    }
                }

                return maxNumber + 1;
            }
            catch
            {
                return 1;
            }
        }

        private string SerializeSession()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(_currentSession, options);
        }

        public ChatSession? LoadChatFromFile(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_chatFilesDirectory, fileName);

                if (!File.Exists(filePath))
                {
                    return null;
                }

                var jsonContent = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<ChatSession>(jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid laddning av chat: {ex.Message}");
                return null;
            }
        }

        public List<string> GetAllChatFiles()
        {
            try
            {
                return Directory.GetFiles(_chatFilesDirectory, "*.json")
                               .Select(Path.GetFileName)
                               .Where(f => f != null)
                               .Cast<string>()
                               .OrderByDescending(f => f)
                               .ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        public void ClearCurrentSession()
        {
            _currentSession.Messages.Clear();
        }
    }
}