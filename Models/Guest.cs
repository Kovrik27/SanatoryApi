using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Guest
{
    public int Id { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Pasport { get; set; } = null!;

    public string Policy { get; set; } = null!;
 
    public DateOnly DataArrival { get; set; }

    public DateOnly DataOfDeparture { get; set; }

    public int RoomId { get; set; }

    public int ProcedureId { get; set; }

    public int? UserId { get; set; }

    public virtual Procedure Procedure { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;

    public virtual User? User { get; set; }
}
