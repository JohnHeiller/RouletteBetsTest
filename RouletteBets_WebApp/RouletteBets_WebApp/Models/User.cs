using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteBets_WebApp.Models
{
    public class User
    {
        public long Id { get; set; }
        public bool? Name { get; set; }
        public string NumberId { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
    }
}
