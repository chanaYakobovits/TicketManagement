using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class AIAnalysisResult
    {
        public int PredictedCategoryId { get; set; }
        public decimal UrgencyScore { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public string AnalysisText { get; set; } = string.Empty;
        public string SuggestedResponse { get; set; } = string.Empty;
    }
}
