using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Event
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int? Times { get; set; }

    public string? Place { get; set; }

    public virtual ICollection<Daytime> Daytimes { get; set; } = new List<Daytime>();
}
