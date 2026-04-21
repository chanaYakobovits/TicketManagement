using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class TicketClosureDTO
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public DateTime ClosedAt { get; set; }

    public string ClosureReason { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public virtual TicketDTO Ticket { get; set; } = null!;
}
