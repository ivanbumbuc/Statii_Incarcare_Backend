using System;
using System.Collections.Generic;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Entities;

public partial class Car
{
    public Guid id { get; set; }

    public Guid user_id { get; set; }

    public string car_plate { get; set; } = null!;

    public virtual User user { get; set; } = null!;
}
