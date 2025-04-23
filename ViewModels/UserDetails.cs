using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TP_WS1.Models;

namespace TP_WS1.ViewModels
{
    public class UserDetails
    {
        [Key,Column(Order=0)]
        public string Username { get; set; }
        public ICollection<AspNetRole> Role { get; set; }
        public int Nb_sujetCreated { get; set; }
        public int Nb_MessageCreated { get; set; }
        public DateTime? LastPostActivity { get; set; }
        public DateTime? LastGameActivity { get; set; }
        public DateTime? LastActivity { get; set; }

        public List<Post> Posts { get; set; }
    }
}