using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Procedure
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? Duration { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();
}
