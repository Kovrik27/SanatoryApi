using System;
using System.Collections.Generic;

namespace SanatoryApi.Models;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public string? Photo { get; set; }

    public int? FeedbackId { get; set; }

    public virtual Feedback? Feedback { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
