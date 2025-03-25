using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Daytime
{
    public int Id { get; set; }

    public DateOnly? Time { get; set; }

    public int? EventId { get; set; }

    public virtual Event? Event { get; set; }
}
