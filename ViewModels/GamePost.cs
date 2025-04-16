using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TP_WS1.Models;

namespace TP_WS1.ViewModels
{
    public class GamePost
    {
        [Key, Column(Order = 0)]

        public string GameName { get; set; }
        public string PostName { get; set; }
        public int PostId { get; set; }
        public int? NbPost { get; set; }
        public int? NbVue { get; set; }
        public string author { get; set; }
        public string lastUserActivity { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}