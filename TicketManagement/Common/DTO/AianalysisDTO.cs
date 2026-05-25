using System;
using System.Collections.Generic;

namespace Common.DTO;

public class AianalysisDTO
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int PredictedCategoryId { get; set; }
    public decimal UrgencyScore { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public string AnalysisText { get; set; } = string.Empty;
    public DateTime AnalyzedAt { get; set; }
}