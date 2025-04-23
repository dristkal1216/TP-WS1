using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Options;
using TP_WS1.Models;

namespace TP_WS1.ViewModels
{
    public class ViewPost
    {
        public int PostId { get; set; }
        [ValidateNever]
        public List<Post> ViewPosts { get; set; } = new();

        public string Message { get; set; } = null!;

        public string? UserId { get; set; }

        public int GameId { get; set; }

        public int? ReactionId { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool IsArchived { get; set; }

        [ValidateNever]
        public virtual Game Game { get; set; } = null!;

        public virtual AspNetUser? User { get; set; } = null!;

    }


}