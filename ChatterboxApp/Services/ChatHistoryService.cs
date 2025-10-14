using ChatterboxApp.Models;
using System.Text.Json;

namespace ChatterboxApp.Services
{
    // Service for managing chat history and file persistence
    public class ChatHistoryService
    {
        private readonly ChatSession _currentSession;
        private readonly string _chatFilesDirectory;

        public ChatHistoryService()
        {
            _currentSession = new ChatSession();

            // ChatFiles directory at same level as Program.cs 
            _chatFilesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "ChatFiles");
            EnsureDirectoryExists();
        }

        // Ensures the ChatFiles directory exists
        private void EnsureDirectoryExists()
        {
            if (!Directory.Exists(_chatFilesDirectory))
            {
                Directory.CreateDirectory(_chatFilesDirectory);
            }
        }

        // Gets the current chat session
        public ChatSession GetCurrentSession()
        {
            return _currentSession;
        }

        // Adds a message to the current session
        public void AddMessage(string role, string content)
        {
            var message = new ChatMessage(role, content);
            if (message.IsValid())
            {
                _currentSession.AddMessage(message);
            }
        }

        // Gets all messages in descending order (newest first)
        public List<ChatMessage> GetMessagesDescending()
        {
            return _currentSession.GetMessagesDescending();
        }

        // Gets all messages in ascending order (oldest first)
        public List<ChatMessage> GetMessagesAscending()
        {
            return _currentSession.Messages.OrderBy(m => m.Timestamp).ToList();
        }

        // Saves all chats to file when app closes 
        public void SaveAllChatsToFile()
        {
            try
            {
                if (_currentSession.GetMessageCount() == 0)
                {
                    return; // No messages to save
                }

                var fileName = GenerateFileName();
                var filePath = Path.Combine(_chatFilesDirectory, fileName);
                var jsonContent = SerializeSession();

                File.WriteAllText(filePath, jsonContent);

                Console.WriteLine($"Chat sparad till: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid sparande av chat: {ex.Message}");
            }
        }

        // Generates a unique filename for the chat history
        private string GenerateFileName()
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            return $"chat_{timestamp}_{_currentSession.SessionId.Substring(0, 8)}.json";
        }

        // Serializes the current session to JSON
        private string SerializeSession()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(_currentSession, options);
        }

        // Loads a chat history from file
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

        // Gets all saved chat files
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

        // Clears the current session
        public void ClearCurrentSession()
        {
            _currentSession.Messages.Clear();
        }
    }
}
