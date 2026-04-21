using System;
using System.Collections.Generic;

namespace Common.Models;

public partial class Category
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Aianalysis> Aianalyses { get; set; } = new List<Aianalysis>();

    public virtual ICollection<Sla> Slas { get; set; } = new List<Sla>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
