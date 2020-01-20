using d6roleplayer.Models;

namespace d6roleplayer.Services
{
    public interface IDiceRollService
    {
        public DiceRollResult GetDiceRollResult(DiceRollRequest request);

        public InitiativeRollResult GetInitiativeRollResult(InitiativeRollRequest request);

        public void ResetInitiativeRollResults(string sessionId);
    }
}
