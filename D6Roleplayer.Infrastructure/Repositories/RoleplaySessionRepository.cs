using System.Threading.Tasks;
using d6roleplayer.Models;
using Microsoft.EntityFrameworkCore;

namespace D6Roleplayer.Infrastructure.Repositories
{
    public class RoleplaySessionRepository : IRoleplaySessionRepository
    {
        private readonly DatabaseContext databaseContext;

        public RoleplaySessionRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task Create(RoleplaySession roleplaySession)
        {
            await databaseContext.AddAsync(roleplaySession);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<RoleplaySession> Read(string sessionId)
        {
            return await databaseContext.RoleplaySessions.FirstOrDefaultAsync(session => session.Id == sessionId);
        }
    }
}
