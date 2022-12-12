using System;
using System.Collections.Generic;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Entities;

public partial class Station
{
    public Guid id { get; set; }

    public string name { get; set; } = null!;

    public string city { get; set; } = null!;

    public string address { get; set; } = null!;

    public decimal coordX { get; set; }

    public decimal coordY { get; set; }

    public virtual ICollection<Plug> Plugs { get; } = new List<Plug>();

    public virtual ICollection<StationToAdmin> StationToAdmins { get; } = new List<StationToAdmin>();
}
