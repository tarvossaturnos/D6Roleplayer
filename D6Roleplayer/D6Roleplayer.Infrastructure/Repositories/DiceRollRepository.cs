using System.Collections.Generic;
using System.Linq;
using D6Roleplayer.Infrastructure.Models;

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

        public void Create(DiceRollResult diceRollResult)
        {
            databaseContext.Add(diceRollResult);
            RemoveDiceEntriesAtLimit(diceRollResult.RoleplaySessionId, 24);

            databaseContext.SaveChanges();
        }

        public IEnumerable<DiceRollResult> Read(string sessionId)
        {
            return databaseContext.DiceRollResults
                .Where(result => result.RoleplaySessionId == sessionId)
                .OrderByDescending(result => result.Id)
                .Take(MaxResults);
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
