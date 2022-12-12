using System;
using System.Collections.Generic;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Entities;

public partial class PlugType
{
    public Guid id { get; set; }

    public string name { get; set; } = null!;

    public int power { get; set; }

    public virtual ICollection<Plug> Plugs { get; } = new List<Plug>();
}
