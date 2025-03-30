using System;
using System.Collections.Generic;

namespace TP_WS1.Models;

public partial class EnumRole
{
    public int Id { get; set; }

    public string Role { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
