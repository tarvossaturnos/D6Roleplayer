using d6roleplayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace D6Roleplayer.Infrastructure.Repositories
{
    public interface IInitiativeRollRepository
    {
        Task Create(InitiativeRollResult initiativeRollResult);

        Task<IEnumerable<InitiativeRollResult>> Read(string sessionId);

        Task Delete(IEnumerable<InitiativeRollResult> initiativeRollResults);
    }
}
