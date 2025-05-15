using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Problem
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public string Place { get; set; } = null!;

    public int StatusProblemId { get; set; }
    public int? StaffId { get; set; }

    public virtual Staff? Staff { get; set; }

    public virtual StatusProblem? StatusProblem { get; set; } = null;
}
