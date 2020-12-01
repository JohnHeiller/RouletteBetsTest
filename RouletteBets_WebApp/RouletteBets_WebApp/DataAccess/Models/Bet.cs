using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteBets_WebApp.Models
{
    public class Bet
    {
        [Key]
        public long Id { get; set; }
        public long? Number { get; set; }
        public bool? Color { get; set; }
        public long Money { get; set; }
        public long UserId { get; set; }
        public long RouletteId { get; set; }
        public string BetTime { get; set; }
        public double? WonMoney { get; set; }
        public bool Active { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("RouletteId")]
        public virtual Roulette Roulette { get; set; }
    }
}
