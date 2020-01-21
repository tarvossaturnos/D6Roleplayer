using Microsoft.EntityFrameworkCore;

namespace D6Roleplayer.Models
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