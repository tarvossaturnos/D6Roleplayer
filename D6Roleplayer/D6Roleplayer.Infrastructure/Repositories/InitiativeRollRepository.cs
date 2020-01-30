using System.Collections.Generic;
using System.Linq;
using D6Roleplayer.Infrastructure.Models;

namespace D6Roleplayer.Infrastructure.Repositories
{
    public class InitiativeRollRepository : IInitiativeRollRepository
    {
        private readonly DatabaseContext databaseContext;

        public InitiativeRollRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public void Create(InitiativeRollResult initiativeRollResult)
        {
            databaseContext.Add(initiativeRollResult);
            databaseContext.SaveChanges();
        }

        public IEnumerable<InitiativeRollResult> Read(string sessionId)
        {
            return databaseContext.InitiativeRollResults
                .Where(result => result.RoleplaySessionId == sessionId)
                .OrderByDescending(result => result.Roll);
        }

        public void Delete(IEnumerable<InitiativeRollResult> initiativeRollResults)
        {
            databaseContext.InitiativeRollResults.RemoveRange(initiativeRollResults);
            databaseContext.SaveChanges();
        }
    }
}
