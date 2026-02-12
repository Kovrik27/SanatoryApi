using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Resource
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public decimal Amount { get; set; }

    public bool IsAvailability { get; set; }
}
