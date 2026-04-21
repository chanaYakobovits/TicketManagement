using System;
using System.Collections.Generic;

namespace Common.Models;

public partial class Aianalysis
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public int PredictedCategoryId { get; set; }

    public decimal UrgencyScore { get; set; }

    public string RiskLevel { get; set; } = null!;

    public string AnalysisText { get; set; } = null!;

    public DateTime AnalyzedAt { get; set; }

    public virtual Category PredictedCategory { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
