using System;
using System.Collections.Generic;

namespace Statii_Incarcare_Proiect_Tehnologii_Web.Entities;

public partial class StationToAdmin
{
    public Guid id { get; set; }

    public Guid station_id { get; set; }

    public Guid admin_id { get; set; }

    public virtual User admin { get; set; } = null!;

    public virtual Station station { get; set; } = null!;
}
