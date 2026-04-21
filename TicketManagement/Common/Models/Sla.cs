using System;
using System.Collections.Generic;

namespace Common.Models;

public partial class Sla
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int UrgencyLevel { get; set; }

    public int TargetResponseHours { get; set; }

    public virtual Category Category { get; set; } = null!;
}
