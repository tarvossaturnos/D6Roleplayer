using System.Collections.Generic;

namespace D6Roleplayer.Models
{
    public class DiceRollsViewModel
    {
        public string RoleplaySessionId { get; set; }

        public string Username { get; set; }

        public IEnumerable<DiceRollResult> DiceRolls { get; set; }

        public IEnumerable<InitiativeRollResult> InitiativeRolls { get; set; }
    }
}
