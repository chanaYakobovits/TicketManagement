using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class UserDTO
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string Password { get; set; }
    public int UserTypeId { get; set; }

    public int DepartmentId { get; set; }

    public string? JobTitle { get; set; }

    public bool Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AttachmentDTO> Attachments { get; set; } = new List<AttachmentDTO>();

    //public virtual DepartmentDTO Department { get; set; } = null!;

    public virtual ICollection<MessageDTO> Messages { get; set; } = new List<MessageDTO>();

    public virtual ICollection<TicketAssignmentDTO> TicketAssignments { get; set; } = new List<TicketAssignmentDTO>();

    public virtual ICollection<TicketStatusHistoryDTO> TicketStatusHistories { get; set; } = new List<TicketStatusHistoryDTO>();

    public virtual ICollection<TicketDTO> Tickets { get; set; } = new List<TicketDTO>();

    //public virtual UserTypeDTO UserType { get; set; } = null!;
}
