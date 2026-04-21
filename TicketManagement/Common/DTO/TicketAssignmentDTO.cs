using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class TicketAssignmentDTO
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public int UserId { get; set; }

    public string RoleInTicket { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual TicketDTO Ticket { get; set; } = null!;

    public virtual UserDTO User { get; set; } = null!;
}
