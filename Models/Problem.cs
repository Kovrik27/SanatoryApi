using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Problem
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public string? Place { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
