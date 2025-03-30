using System;
using System.Collections.Generic;

namespace TP_WS1.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public int UserId { get; set; }

    public int GameId { get; set; }

    public int? ReactionId { get; set; }

    public bool IsArchived { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
