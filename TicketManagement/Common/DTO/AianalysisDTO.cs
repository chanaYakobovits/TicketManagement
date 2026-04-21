using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class AianalysisDTO
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public int PredictedCategoryId { get; set; }

    public decimal UrgencyScore { get; set; }

    public string RiskLevel { get; set; } = null!;

    public string AnalysisText { get; set; } = null!;

    public DateTime AnalyzedAt { get; set; }

    public virtual CategoryDTO PredictedCategory { get; set; } = null!;

    public virtual TicketDTO Ticket { get; set; } = null!;
}
