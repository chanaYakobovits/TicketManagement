using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class TicketDTO

{
    public int Id { get; set; }
    public string? Subject { get; set; }
    public string Description { get; set; } = null!;
    public int CategoryId { get; set; }
    public int UrgencyLevel { get; set; }
    public string CurrentStatus { get; set; } = string.Empty;
    public int OpenedByUserId { get; set; }
    public DateOnly OpenedDate { get; set; }
    public TimeOnly OpenedTime { get; set; }
}