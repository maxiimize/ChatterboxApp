using Chatterbox.Models;

namespace ChatterboxApp.Models
{

    // Summarize a complete chat session with history
    public class ChatSession
    {
        public string SessionId { get; set; }
        public List<ChatMessage> Messages { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }

        public ChatSession()
        {
            SessionId = Guid.NewGuid().ToString();
            Messages = new List<ChatMessage>();
            CreatedAt = DateTime.Now;
            LastUpdated = DateTime.Now;
        }
       
        public void AddMessage(ChatMessage message)
        {
            if (message != null && message.IsValid())
            {
                Messages.Add(message);
                LastUpdated = DateTime.Now;
            }
        }
       
        public List<ChatMessage> GetMessagesDescending()
        {
            return Messages.OrderByDescending(m => m.Timestamp).ToList();
        }

        
        public int GetMessageCount()
        {
            return Messages.Count;
        }
    }
}
