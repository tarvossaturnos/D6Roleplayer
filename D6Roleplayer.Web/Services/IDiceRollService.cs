using d6roleplayer.Models;
using System.Threading.Tasks;

namespace d6roleplayer.Services
{
    public interface IDiceRollService
    {
        public DiceRollResult GetDiceRollResult(DiceRollRequest request);

        public InitiativeRollResult GetInitiativeRollResult(InitiativeRollRequest request);

        public Task ResetInitiativeRollResults(string sessionId);
    }
}
