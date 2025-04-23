using System;
using System.Collections.Generic;

namespace TP_WS1.Models;

public partial class Post
{
    public int PostId { get; set; }
    public string Message { get; set; } = null!;

    public string? UserId { get; set; }

    public int GameId { get; set; }

    public int? ReactionId { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsArchived { get; set; }

    public int? Click { get; set; } = null!;

    public virtual Game Game { get; set; } = null!;

    public virtual AspNetUser? User { get; set; } = null!;
}
