using D6Roleplayer.Infrastructure.Models;
using D6Roleplayer.Web.Models;

namespace D6Roleplayer.Web.Services
{
    public interface IDiceRollService
    {
        public DiceRollResult GetDiceRollResult(DiceRollRequest request);

        public InitiativeRollResult GetInitiativeRollResult(InitiativeRollRequest request);

        public void ResetInitiativeRollResults(string sessionId);
    }
}
