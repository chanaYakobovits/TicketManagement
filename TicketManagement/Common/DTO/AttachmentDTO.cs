using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class AttachmentDTO
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public int UploadedByUserId { get; set; }

    public string FileName { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public virtual TicketDTO Ticket { get; set; } = null!;

    public virtual UserDTO UploadedByUser { get; set; } = null!;
}
