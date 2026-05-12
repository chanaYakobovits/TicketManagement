using System;
using System.Collections.Generic;

namespace Common.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public DateOnly OpenedDate { get; set; }

    public TimeOnly OpenedTime { get; set; }
    public string ? Subject { get; set; }
    public string Description { get; set; } = null!;

    public int CategoryId { get; set; }

    public int UrgencyLevel { get; set; }

    public string CurrentStatus { get; set; } = null!;

    public int OpenedByUserId { get; set; }

    public virtual ICollection<Aianalysis> Aianalyses { get; set; } = new List<Aianalysis>();

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual User OpenedByUser { get; set; } = null!;

    public virtual ICollection<TicketAssignment> TicketAssignments { get; set; } = new List<TicketAssignment>();

    public virtual TicketClosure? TicketClosure { get; set; }

    public virtual ICollection<TicketStatusHistory> TicketStatusHistories { get; set; } = new List<TicketStatusHistory>();
}
