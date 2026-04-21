using System;
using System.Collections.Generic;

namespace Common.Models;

public partial class Attachment
{
    public int Id { get; set; }

    public int TicketId { get; set; }

    public int UploadedByUserId { get; set; }

    public string FileName { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;

    public virtual User UploadedByUser { get; set; } = null!;
}
