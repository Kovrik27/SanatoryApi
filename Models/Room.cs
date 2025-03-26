using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Room
{
    public int Id { get; set; }

    public int Number { get; set; }

    public string Type { get; set; } = null!;

    public decimal Price { get; set; }

    public int StatusId { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();

    public virtual Status Status { get; set; } = null!;
}
