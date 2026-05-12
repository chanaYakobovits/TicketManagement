using Common.DTO;
using Common.Models;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly BaseRepository<Category> _categoryRepository;
        private readonly string _apiKey;

        public AIService(HttpClient httpClient, BaseRepository<Category> categoryRepository, IConfiguration config)
        {
            _httpClient = httpClient;
            _categoryRepository = categoryRepository;
            _apiKey = config["Anthropic:ApiKey"]!;
        }

        public async Task<AIAnalysisResult> AnalyzeTicket(string subject, string description)
        {
            // שליפת הקטגוריות מהמסד
            var categories = await _categoryRepository.Items.ToListAsync();
            var categoryList = string.Join("\n", categories.Select(c => $"- Id: {c.Id}, שם: {c.CategoryName}"));

            var prompt = $@"אתה מערכת AI לניתוח פניות עובדים.

            הקטגוריות האפשריות הן:
            {categoryList}

            נתח את הפנייה הבאה והחזר JSON בלבד ללא טקסט נוסף:

            נושא: {subject}
            תיאור: {description}

            החזר בפורמט הבא בדיוק:
            {{
              ""predictedCategoryId"": <מספר Id של הקטגוריה המתאימה ביותר>,
              ""urgencyScore"": <מספר בין 1 ל-4 כאשר 1=נמוכה 2=בינונית 3=גבוהה 4=קריטית>,
              ""riskLevel"": <Low או Medium או High>,
              ""analysisText"": <הסבר קצר בעברית על הניתוח>,
              ""suggestedResponse"": <תשובה מוצעת לעובד בעברית>
            }}";

            var requestBody = new
            {
                model = "claude-haiku-4-5-20251001",
                max_tokens = 1000,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages");
            request.Headers.Add("x-api-key", _apiKey);
            request.Headers.Add("anthropic-version", "2023-06-01");
            request.Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            var parsed = JsonSerializer.Deserialize<JsonElement>(responseBody);
            var text = parsed
                .GetProperty("content")[0]
                .GetProperty("text")
                .GetString()!;

            var result = JsonSerializer.Deserialize<AIAnalysisResult>(text, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

            return result;
        }
    }
}
