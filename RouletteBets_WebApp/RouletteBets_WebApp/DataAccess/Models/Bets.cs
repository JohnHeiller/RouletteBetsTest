using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteBets_WebApp.Models
{
    public class Bets
    {
        [Key]
        public long Id { get; set; }
        public long? Number { get; set; }
        public bool? Color { get; set; }
        public long Money { get; set; }
        public long UserId { get; set; }
        public DateTime BetTime { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
