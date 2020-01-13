using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using d6roleplayer.Models;
using Microsoft.EntityFrameworkCore;

namespace D6Roleplayer.Infrastructure.Repositories
{
    public class DiceRollRepository : IDiceRollRepository
    {
        private readonly DatabaseContext databaseContext;

        const int MaxResults = 25;

        public DiceRollRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task Create(DiceRollResult diceRollResult)
        {
            await databaseContext.AddAsync(diceRollResult);
            RemoveDiceEntriesAtLimit(diceRollResult.RoleplaySessionId, 24);

            await databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<DiceRollResult>> Read(string sessionId)
        {
            return await databaseContext.DiceRollResults
                .Where(result => result.RoleplaySessionId == sessionId)
                .OrderByDescending(result => result.Id)
                .Take(MaxResults)
                .ToListAsync();
        }

        private void RemoveDiceEntriesAtLimit(string sessionId, int limit)
        {
            var entriesToDelete = databaseContext.DiceRollResults
                .Where(diceRoll => diceRoll.RoleplaySessionId == sessionId)
                .OrderByDescending(result => result.Id)
                .Skip(limit);

            foreach (var entry in entriesToDelete)
            {
                databaseContext.Remove(entry);
            }
        }
    }
}
