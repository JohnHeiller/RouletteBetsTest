using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteBets_WebApp.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string NumberId { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public long CreditMoney { get; set; }
    }
}
