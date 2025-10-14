using ChatterboxApp.Models;

namespace ChatterboxApp.Services
{
    // Interface for Azure OpenAI service operations
    public interface IAzureOpenAIService
    {
        Task<string> GetChatResponseAsync(string userMessage, List<ChatMessage> conversationHistory);

        bool IsConfigured();
    }
}
