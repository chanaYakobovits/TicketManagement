using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class TicketDetailDTO
    {
        public int Id { get; set; }
        public string? Subject { get; set; }
        public string Description { get; set; } = null!;
        public int CategoryId { get; set; }
        public int UrgencyLevel { get; set; }
        public string CurrentStatus { get; set; } = null!;
        public int OpenedByUserId { get; set; }
        public DateOnly OpenedDate { get; set; }
        public TimeOnly OpenedTime { get; set; }
        public CategoryDTO? Category { get; set; }
        public List<AttachmentDTO> Attachments { get; set; } = new();
        public List<MessageDTO> Messages { get; set; } = new();
        public List<TicketStatusHistoryDTO> TicketStatusHistories { get; set; } = new();
        public List<TicketAssignmentDTO> TicketAssignments { get; set; } = new();
        public List<AianalysisDTO> Aianalyses { get; set; } = new();
    }
}
