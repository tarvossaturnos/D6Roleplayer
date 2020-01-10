using d6roleplayer.Models;
using Microsoft.EntityFrameworkCore;

namespace d6roleplayer.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) { }

        public DbSet<DiceRollResult> DiceRollResults { get; set; }
        public DbSet<InitiativeRollResult> InitiativeRollResults { get; set; }
        public DbSet<RoleplaySession> RoleplaySessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}