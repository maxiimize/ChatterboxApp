using System.Text;
using System.Text.Json;
using ChatterboxApp.Models;

namespace ChatterboxApp.Services
{
    // Service for communicating with Azure OpenAI
    public class AzureOpenAIService : IAzureOpenAIService
    {
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly string _deploymentName;
        private readonly HttpClient _httpClient;

        public AzureOpenAIService()
        {
            // Get API key from environment variable
            _apiKey = Environment.GetEnvironmentVariable("OPENAIKEY") ?? string.Empty;

            _endpoint = "https://chatterboxapp.openai.azure.com/";
            _deploymentName = "chatterbox-gpt35";

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);
        }

        public bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(_apiKey) &&
                   !string.IsNullOrWhiteSpace(_endpoint);
        }

        public async Task<string> GetChatResponseAsync(string userMessage, List<ChatMessage> conversationHistory)
        {
            if (!IsConfigured())
            {
                throw new InvalidOperationException("Azure OpenAI är inte konfigurerad korrekt");
            }

            if (string.IsNullOrWhiteSpace(userMessage))
            {
                throw new ArgumentException("Meddelandet får inte vara tomt");
            }

            try
            {
                var messages = BuildMessageList(userMessage, conversationHistory);
                var requestBody = CreateRequestBody(messages);
                var response = await SendRequestAsync(requestBody);

                return ExtractResponseContent(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"Fel vid kommunikation med Azure OpenAI: {ex.Message}", ex);
            }
        }

        // Builds the complete message list including system prompt
        private List<object> BuildMessageList(string userMessage, List<ChatMessage> history)
        {
            var messages = new List<object>
            {
                // System prompt - Chatterbox persona
                new
                {
                    role = "system",
                    content = @"Du är Chatterbox, en 55-årig kundtjänstmedarbetare på Nätonnät. 
                    Du bor i Stockholm och är expert på hemelektronik. 
                    Du hjälper ENDAST med frågor om hemelektronik som TV, datorer, mobiler, 
                    hushållsapparater, ljudutrustning, etc. 
                    Om någon frågar om något annat än hemelektronik, förklara vänligt att 
                    du endast kan hjälpa till med hemelektronik-relaterade frågor.
                    Var professionell men vänlig i din ton."
                }
            };

            // Add conversation history (limited to last 10 messages for context)
            var recentHistory = history.OrderBy(m => m.Timestamp).TakeLast(10);
            foreach (var msg in recentHistory)
            {
                messages.Add(new
                {
                    role = msg.Role,
                    content = msg.Content
                });
            }

            // Add current user message
            messages.Add(new
            {
                role = "user",
                content = userMessage
            });

            return messages;
        }


        private object CreateRequestBody(List<object> messages)
        {
            return new
            {
                messages = messages,
                max_tokens = 800,
                temperature = 0.7,
                top_p = 0.95
            };
        }

        private async Task<string> SendRequestAsync(object requestBody)
        {
            var url = $"{_endpoint}/openai/deployments/{_deploymentName}/chat/completions?api-version=2024-02-15-preview";
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API-anrop misslyckades: {response.StatusCode} - {errorContent}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        private string ExtractResponseContent(string jsonResponse)
        {
            using var doc = JsonDocument.Parse(jsonResponse);
            var root = doc.RootElement;

            if (root.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
            {
                var firstChoice = choices[0];
                if (firstChoice.TryGetProperty("message", out var message) &&
                    message.TryGetProperty("content", out var content))
                {
                    return content.GetString() ?? "Inget svar mottaget";
                }
            }

            throw new Exception("Kunde inte extrahera svar från API-svaret");
        }
    }
}
