using D6Roleplayer.Infrastructure.Models;
using System.Collections.Generic;

namespace D6Roleplayer.Infrastructure.Repositories
{
    public interface IDiceRollRepository
    {
        void Create(DiceRollResult diceRollResult);

        IEnumerable<DiceRollResult> Read(string sessionId);
    }
}
