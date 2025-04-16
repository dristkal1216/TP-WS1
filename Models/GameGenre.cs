using System;
using System.Collections.Generic;

namespace TP_WS1.Models;

public partial class GameGenre
{
    public string GameGenreId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public bool IsArchived { get; set; }

    public string? Userid { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual AspNetUser? User { get; set; }
}
