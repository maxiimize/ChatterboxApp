using ChatterboxApp.Models;
using ChatterboxApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Chatterbox.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IAzureOpenAIService _aiService;
        private readonly ChatHistoryService _historyService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(
            IAzureOpenAIService aiService,
            ChatHistoryService historyService,
            ILogger<ChatController> logger)
        {
            _aiService = aiService;
            _historyService = historyService;
            _logger = logger;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            try
            {
                // Validate request 
                if (!ModelState.IsValid || !request.IsValid())
                {
                    return BadRequest(new { error = "Ogiltigt meddelande" });
                }

                // Check if Azure OpenAI is configured
                if (!_aiService.IsConfigured())
                {
                    return StatusCode(500, new { error = "Azure OpenAI är inte konfigurerad" });
                }

                // Get sanitized message
                var userMessage = request.GetSanitizedMessage();

                // Add user message to history
                _historyService.AddMessage("user", userMessage);

                // Get conversation history
                var history = _historyService.GetMessagesAscending();

                // Get AI response
                var aiResponse = await _aiService.GetChatResponseAsync(userMessage, history);

                // Add AI response to history
                _historyService.AddMessage("assistant", aiResponse);

                // Return response with updated chat history
                return Ok(new
                {
                    success = true,
                    userMessage = userMessage,
                    aiResponse = aiResponse,
                    timestamp = DateTime.Now,
                    chatHistory = _historyService.GetMessagesDescending()
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input from user");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing chat message");
                return StatusCode(500, new { error = "Ett fel uppstod vid bearbetning av meddelandet" });
            }
        }

        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            try
            {
                var messages = _historyService.GetMessagesDescending();
                return Ok(new
                {
                    success = true,
                    messages = messages,
                    count = messages.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving chat history");
                return StatusCode(500, new { error = "Ett fel uppstod vid hämtning av chatthistorik" });
            }
        }

        [HttpPost("clear")]
        public IActionResult ClearHistory()
        {
            try
            {
                _historyService.ClearCurrentSession();
                return Ok(new { success = true, message = "Chatthistorik rensad" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing chat history");
                return StatusCode(500, new { error = "Ett fel uppstod vid rensning av chatthistorik" });
            }
        }

      
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                status = "healthy",
                configured = _aiService.IsConfigured(),
                timestamp = DateTime.Now
            });
        }
    }
}