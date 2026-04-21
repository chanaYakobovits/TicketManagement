using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class TicketStatusHistoryDTO
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public string PreviousStatus { get; set; } = null!;

    public string NewStatus { get; set; } = null!;

    public DateTime ChangedAt { get; set; }

    public int ChangedByUserId { get; set; }

    public string? Comment { get; set; }

    public virtual UserDTO ChangedByUser { get; set; } = null!;

    public virtual TicketDTO Ticket { get; set; } = null!;
}
