using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Room
{
    public int Id { get; set; }

    public int? Number { get; set; }

    public string? Type { get; set; }

    public decimal? Price { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();
}
