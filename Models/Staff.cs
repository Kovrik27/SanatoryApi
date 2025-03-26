using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Staff
{
    public int Id { get; set; }

    public string Lastname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string JobTitle { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public int? ProblemId { get; set; }

    public int? CabinetId { get; set; }

    public int? WorkDaysId { get; set; }

    public virtual Cabinet? Cabinet { get; set; }

    public virtual Problem? Problem { get; set; }

    public virtual Day? WorkDays { get; set; }
}
