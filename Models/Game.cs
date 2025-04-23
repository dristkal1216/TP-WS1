using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TP_WS1.Models;

public partial class Game
{
    public int GameId { get; set; }

    public string Name { get; set; } = null!;

    public string GameGenreId { get; set; } = null!;

    public bool IsOnline { get; set; }

    public string GameEngine { get; set; } = null!;

    public string? UserId { get; set; }

    public bool IsArchived { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    [ValidateNever]
    public virtual GameGenre GameGenre { get; set; } = null!;

    [ValidateNever]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual AspNetUser? User { get; set; }
}
