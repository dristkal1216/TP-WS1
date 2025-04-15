using System;
using System.Collections.Generic;

namespace TP_WS1.Models;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Username { get; set; } = null!;

    public int PostId { get; set; }

    public DateTime? LastActivity { get; set; }
}
