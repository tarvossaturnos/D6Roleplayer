using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using d6roleplayer.Models;
using Microsoft.EntityFrameworkCore;

namespace D6Roleplayer.Infrastructure.Repositories
{
    public class InitiativeRollRepository : IInitiativeRollRepository
    {
        private readonly DatabaseContext databaseContext;

        public InitiativeRollRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task Create(InitiativeRollResult initiativeRollResult)
        {
            await databaseContext.AddAsync(initiativeRollResult);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<InitiativeRollResult>> Read(string sessionId)
        {
            return await databaseContext.InitiativeRollResults
                .Where(result => result.RoleplaySessionId == sessionId)
                .OrderByDescending(result => result.Roll)
                .ToListAsync();
        }

        public async Task Delete(IEnumerable<InitiativeRollResult> initiativeRollResults)
        {
            databaseContext.InitiativeRollResults.RemoveRange(initiativeRollResults);
            await databaseContext.SaveChangesAsync();
        }
    }
}
