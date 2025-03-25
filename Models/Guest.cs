using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Guest
{
    public int Id { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? Lastname { get; set; }

    public string? Pasport { get; set; }

    public string? Policy { get; set; }

    public DateOnly? DataArrival { get; set; }

    public DateOnly? DataOfDeparture { get; set; }

    public int? RoomId { get; set; }

    public int? ProcedureId { get; set; }

    public virtual Procedure? Procedure { get; set; }

    public virtual Room? Room { get; set; }
}
