using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP_WS1.Models;

public partial class GameGenre
{
    [Key,Column(Order = 0)]
    public string GameGenreId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public bool IsArchived { get; set; }

    public string? UserId { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual AspNetUser? User { get; set; }
}
