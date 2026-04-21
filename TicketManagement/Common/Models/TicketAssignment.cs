using System;
using System.Collections.Generic;

namespace Common.Models;

public partial class TicketAssignment
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public int UserId { get; set; }

    public string RoleInTicket { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
