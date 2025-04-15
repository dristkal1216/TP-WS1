using System;
using System.Collections.Generic;

namespace TP_WS1.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public int PostId { get; set; }

    public DateTime? LastActivity { get; set; }

    public virtual AspNetUser? AspNetUser { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
