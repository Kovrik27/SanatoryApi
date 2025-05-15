using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Daytime
{
    public int Id { get; set; }

    public DateTime Time { get; set; } = DateTime.Now;

    public virtual ICollection<Event>? Events { get; set; } = new List<Event>();
}
