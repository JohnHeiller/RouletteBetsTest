using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteBets_WebApp.DTO
{
    public class BetValidationsDTO
    {
        public long MinNumber { get; set; }
        public long MaxNumber { get; set; }
        public long MaxMoney { get; set; }
    }
}
