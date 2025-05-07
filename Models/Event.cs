using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int Duration { get; set; }

    public string Place { get; set; } = null!;

    public string? Date { get; set; }

    public virtual ICollection<Daytime> Days { get; set; } = new List<Daytime>();
}
