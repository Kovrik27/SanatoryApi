using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class PriorityProblem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Problem> Problems { get; set; } = new List<Problem>();
}
