using System;
using System.Collections.Generic;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Entities;

public partial class User
{
    public Guid id { get; set; }

    public string name { get; set; } = null!;

    public string password { get; set; } = null!;

    public string is_admin { get; set; } = null!;

    public string is_charging { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; } = new List<Booking>();

    public virtual ICollection<Car> Cars { get; } = new List<Car>();

    public virtual ICollection<StationToAdmin> StationToAdmins { get; } = new List<StationToAdmin>();
}
