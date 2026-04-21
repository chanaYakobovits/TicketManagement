using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class SlaDTO
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int UrgencyLevel { get; set; }

    public int TargetResponseHours { get; set; }

    public virtual CategoryDTO Category { get; set; } = null!;
}
