using System;
using System.Collections.Generic;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Entities;

public partial class Booking
{
    public Guid id { get; set; }

    public DateTime start_time { get; set; }

    public DateTime end_time { get; set; }

    public Guid plug_id { get; set; }

    public Guid user_id { get; set; }

    public virtual Plug plug { get; set; } = null!;

    public virtual User user { get; set; } = null!;
}
