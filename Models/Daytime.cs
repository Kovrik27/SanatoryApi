using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Daytime
{
    public int Id { get; set; }

    public DateTime Time { get; set; }

    public int EventId { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
