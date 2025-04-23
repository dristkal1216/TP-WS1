using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TP_WS1.Models;

namespace TP_WS1.ViewModels
{
    [NotMapped]
    public class GGenreGame
    {
        [Key, Column(Order = 0)]
        public string fullName { get; set; }
        public List<Game> Top3Games { get; set; }

        public string GameGenreId { get; set; }

    }

} 