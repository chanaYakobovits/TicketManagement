using System;
using System.Collections.Generic;

namespace Common.DTO;

public partial class CategoryDTO
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<AianalysisDTO> Aianalyses { get; set; } = new List<AianalysisDTO>();

    public virtual ICollection<SlaDTO> Slas { get; set; } = new List<SlaDTO>();

    public virtual ICollection<TicketDTO> Tickets { get; set; } = new List<TicketDTO>();
}
