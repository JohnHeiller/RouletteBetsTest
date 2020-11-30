using Microsoft.EntityFrameworkCore;
using RouletteBets_WebApp.Models;

namespace RouletteBets_WebApp.DataAccess
{
    public class RouletteBetsContext : DbContext
    {
        public RouletteBetsContext(DbContextOptions<RouletteBetsContext> options)
            : base(options)
        {
        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<Roulette> Roulette { get; set; }
        public DbSet<User> User { get; set; }
    }
}