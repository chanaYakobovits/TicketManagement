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
            _apiKey = config["OpenAI:ApiKey"]!;
        }

        public async Task<AIAnalysisResult> AnalyzeTicket(string subject, string description)
        {
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
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "אתה מערכת ניתוח פניות. החזר תמיד JSON בלבד." },
                    new { role = "user", content = prompt }
                },
                max_tokens = 1000,
                temperature = 0.3
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            var parsed = JsonSerializer.Deserialize<JsonElement>(responseBody);
            var text = parsed
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()!;

            // ניקוי אם ה-AI החזיר ```json
            var cleanText = text
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            var result = JsonSerializer.Deserialize<AIAnalysisResult>(cleanText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

            return result;
        }
    }
}
