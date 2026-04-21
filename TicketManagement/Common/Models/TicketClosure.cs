using System;
using System.Collections.Generic;

namespace Common.Models;

public partial class TicketClosure
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public DateTime ClosedAt { get; set; }

    public string ClosureReason { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
