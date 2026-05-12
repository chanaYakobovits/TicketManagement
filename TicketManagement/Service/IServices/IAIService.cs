using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IAIService
    {
        Task<AIAnalysisResult> AnalyzeTicket(string subject, string description);
    }
}
