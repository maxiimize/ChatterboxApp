namespace Chatterbox.Models
{
    //Represents a single chat message in the conversation
    public class ChatMessage
    {
        public string Role { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }

        public ChatMessage()
        {
            Timestamp = DateTime.Now;
        }

        public ChatMessage(string role, string content)
        {
            Role = role;
            Content = content;
            Timestamp = DateTime.Now;
        }

        //Validates if the message has valid content
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Role) &&
                   !string.IsNullOrWhiteSpace(Content) &&
                   (Role == "user" || Role == "assistant" || Role == "system");
        }
    }
}