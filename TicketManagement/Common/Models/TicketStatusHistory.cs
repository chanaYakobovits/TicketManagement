using System;
using System.Collections.Generic;

namespace Common.Models;

public partial class TicketStatusHistory
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public string PreviousStatus { get; set; } = null!;

    public string NewStatus { get; set; } = null!;

    public DateTime ChangedAt { get; set; }

    public int ChangedByUserId { get; set; }

    public string? Comment { get; set; }

    public virtual User ChangedByUser { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
