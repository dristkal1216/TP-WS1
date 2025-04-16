using System;
using System.Collections.Generic;

namespace TP_WS1.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public int GameId { get; set; }

    public int? ReactionId { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsArchived { get; set; }

    public int? Click { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
