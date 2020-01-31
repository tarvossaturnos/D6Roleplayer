using D6Roleplayer.Infrastructure.Models;
using System.Collections.Generic;

namespace D6Roleplayer.Infrastructure.Repositories
{
    public interface IInitiativeRollRepository
    {
        void Create(InitiativeRollResult initiativeRollResult);

        IEnumerable<InitiativeRollResult> Read(string sessionId);

        void Delete(IEnumerable<InitiativeRollResult> initiativeRollResults);
    }
}
