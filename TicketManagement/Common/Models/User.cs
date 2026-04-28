using System;
using System.Collections.Generic;

namespace Common.Models;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;
    public int EmployeeId { get; set; }
    public string? Phone { get; set; }

    public string Password { get; set; }
    public int UserTypeId { get; set; }

    public int DepartmentId { get; set; }

    public string? JobTitle { get; set; }

    public bool Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<TicketAssignment> TicketAssignments { get; set; } = new List<TicketAssignment>();

    public virtual ICollection<TicketStatusHistory> TicketStatusHistories { get; set; } = new List<TicketStatusHistory>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual UserType UserType { get; set; } = null!;
}
