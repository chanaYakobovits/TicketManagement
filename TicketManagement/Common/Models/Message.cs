using System;
using System.Collections.Generic;

namespace Common.Models;

public partial class Message
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
