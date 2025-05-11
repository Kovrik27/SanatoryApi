using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new ();
}
