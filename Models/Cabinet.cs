using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Cabinet
{
    public int Id { get; set; }

    public int? Number { get; set; }

    public string? Type { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
