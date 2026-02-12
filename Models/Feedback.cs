using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public int Mark { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
