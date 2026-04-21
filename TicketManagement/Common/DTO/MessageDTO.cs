using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class MessageDTO
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public virtual TicketDTO Ticket { get; set; } = null!;

    public virtual UserDTO User { get; set; } = null!;
}
