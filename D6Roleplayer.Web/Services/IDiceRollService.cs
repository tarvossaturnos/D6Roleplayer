using D6Roleplayer.Models;

namespace D6Roleplayer.Services
{
    public interface IDiceRollService
    {
        public DiceRollResult GetDiceRollResult(DiceRollRequest request);

        public InitiativeRollResult GetInitiativeRollResult(InitiativeRollRequest request);

        public void ResetInitiativeRollResults(string sessionId);
    }
}
