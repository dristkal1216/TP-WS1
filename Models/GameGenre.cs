using System;
using System.Collections.Generic;

namespace TP_WS1.Models;

public partial class GameGenre
{
    public string GameGenreId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public int? GameTypeId { get; set; }

    public bool IsArchived { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
