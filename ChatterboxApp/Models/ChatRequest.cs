using System.ComponentModel.DataAnnotations;

namespace ChatterboxApp.Models
{
    public class ChatRequest
    {
        [Required(ErrorMessage = "Meddelande är obligatoriskt")]
        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Meddelandet måste vara mellan 1 och 2000 tecken")]
        public string Message { get; set; } = string.Empty;

        
        public string GetSanitizedMessage()
        {
            if (string.IsNullOrWhiteSpace(Message))
                return string.Empty;

            // Remove potentially harmful characters
            return Message.Trim()
                         .Replace("<script>", "")
                         .Replace("</script>", "")
                         .Replace("<", "&lt;")
                         .Replace(">", "&gt;");
        }

        
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Message) &&
                   Message.Length >= 1 &&
                   Message.Length <= 2000;
        }
    }
}
