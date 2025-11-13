using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;


namespace Infrastructure.AiAssistant;

public class GroqAIAssistantService : IAiAssistantService
{
    private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GroqAIAssistantService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Groq:ApiKey"]
                ?? throw new InvalidOperationException("Groq API key is missing in configuration.");
        }

        public async Task<string> GetAnswerAsync(string userQuery, string contextData, CancellationToken cancellationToken)
        {
            var prompt = BuildPrompt(userQuery, contextData);

            var requestBody = new
            {
                model = "openai/gpt-oss-20b",
                messages = new[]
                {
                    new { role = "system", content = "You are an AI assistant that answers questions about events. You have read-only access to event data. Never modify or create any data." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.4
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.groq.com/openai/v1/chat/completions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            request.Content = content;

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var jsonResponse = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            var answer = jsonResponse.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return string.IsNullOrWhiteSpace(answer)
                ? "Sorry, I didn’t understand that. Please try rephrasing your question."
                : answer.Trim();
        }

        private static string BuildPrompt(string userQuery, string contextData)
        {
            var sb = new StringBuilder();
            sb.AppendLine("You are an event assistant. Use the context below to answer the question accurately.");
            sb.AppendLine("Context data (JSON):");
            sb.AppendLine(contextData);
            sb.AppendLine();
            sb.AppendLine("User question:");
            sb.AppendLine(userQuery);
            sb.AppendLine();
            sb.AppendLine("Answer clearly and concisely.");
            return sb.ToString();
        }
    }