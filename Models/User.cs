using System;
using System.Collections.Generic;

namespace TP_WS1.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int PostId { get; set; }

    public int Role { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual EnumRole RoleNavigation { get; set; } = null!;
}
