using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class TicketDTO
{
    public int Id { get; set; }

    public DateOnly OpenedDate { get; set; }

    public TimeOnly OpenedTime { get; set; }

    public string Description { get; set; } = null!;

    public int CategoryId { get; set; }

    public int UrgencyLevel { get; set; }

    public string CurrentStatus { get; set; } = null!;

    public int OpenedByUserId { get; set; }

    public virtual ICollection<AianalysisDTO> Aianalyses { get; set; } = new List<AianalysisDTO>();

    public virtual ICollection<AttachmentDTO> Attachments { get; set; } = new List<AttachmentDTO>();

    public virtual CategoryDTO Category { get; set; } = null!;

    public virtual ICollection<MessageDTO> Messages { get; set; } = new List<MessageDTO>();

    public virtual UserDTO OpenedByUser { get; set; } = null!;

    public virtual ICollection<TicketAssignmentDTO> TicketAssignments { get; set; } = new List<TicketAssignmentDTO>();

    public virtual TicketClosureDTO? TicketClosure { get; set; }

    public virtual ICollection<TicketStatusHistoryDTO> TicketStatusHistories { get; set; } = new List<TicketStatusHistoryDTO>();
}
