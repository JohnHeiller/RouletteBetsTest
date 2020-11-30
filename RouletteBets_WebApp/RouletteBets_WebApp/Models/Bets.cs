using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteBets_WebApp.Models
{
    public class Bets
    {
        public long? Number { get; set; }
        public bool? Color { get; set; }
        public long Money { get; set; }
        public long UserId { get; set; }
        public DateTime BetTime { get; set; }
    }
}
