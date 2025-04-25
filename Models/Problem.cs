using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Problem
{
    public int? Id { get; set; }

    public string? Description { get; set; } = null!;

    public string? Place { get; set; } = null!;

    public int StatusProblem { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    public virtual StatusProblem StatusProblemNavigation { get; set; } = null!;
}
