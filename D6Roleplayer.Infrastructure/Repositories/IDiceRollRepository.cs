using d6roleplayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D6Roleplayer.Infrastructure.Repositories
{
    public interface IDiceRollRepository
    {
        Task Create(DiceRollResult diceRollResult);

        Task<IEnumerable<DiceRollResult>> Read(string sessionId);
    }
}
