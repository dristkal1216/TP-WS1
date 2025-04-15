using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TP_WS1.Models;

namespace TP_WS1.ViewModels
{
    internal class GamePost
    {
        [Key, Column(Order = 0)]
        public string GameName { get; set; }
        public int nbPost { get; set; }
        public object nbVue { get; set; }
        public string author { get; set; }
        public string lastUserActivity { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}