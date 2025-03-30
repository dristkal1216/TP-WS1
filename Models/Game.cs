using System;
using System.Collections.Generic;

namespace TP_WS1.Models;

public partial class Game
{
    public int GameId { get; set; }

    public string Name { get; set; } = null!;

    public string GameGenreId { get; set; } = null!;

    public bool IsOnline { get; set; }

    public string GameEngine { get; set; } = null!;

    public int? UserId { get; set; }

    public bool IsArchived { get; set; }

    public virtual GameGenre GameGenre { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual User? User { get; set; }
}
