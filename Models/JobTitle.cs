﻿using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class JobTitle
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
