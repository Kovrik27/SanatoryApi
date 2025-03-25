using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Day
{
    public int Id { get; set; }

    public string? Day1 { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
