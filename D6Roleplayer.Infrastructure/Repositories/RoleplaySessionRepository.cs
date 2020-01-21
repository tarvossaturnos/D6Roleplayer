using System.Linq;
using D6Roleplayer.Infrastructure.Models;

namespace D6Roleplayer.Infrastructure.Repositories
{
    public class RoleplaySessionRepository : IRoleplaySessionRepository
    {
        private readonly DatabaseContext databaseContext;

        public RoleplaySessionRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public void Create(RoleplaySession roleplaySession)
        {
            databaseContext.Add(roleplaySession);
            databaseContext.SaveChanges();
        }

        public RoleplaySession Read(string sessionId)
        {
            return databaseContext.RoleplaySessions.FirstOrDefault(session => session.Id == sessionId);
        }
    }
}
