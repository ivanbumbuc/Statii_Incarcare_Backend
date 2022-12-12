using System;
using System.Collections.Generic;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Entities;

public partial class Plug
{
    public Guid id { get; set; }

    public Guid station_id { get; set; }

    public Guid type { get; set; }

    public string is_charging { get; set; } = null!;

    public decimal price { get; set; }

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual Station station { get; set; } = null!;

    public virtual PlugType typeNavigation { get; set; } = null!;
}
