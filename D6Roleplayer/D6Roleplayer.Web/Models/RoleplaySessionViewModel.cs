using D6Roleplayer.Infrastructure.Models;
using System.Collections.Generic;

namespace D6Roleplayer.Web.Models
{
    public class RoleplaySessionViewModel
    {
        public string RoleplaySessionId { get; set; }

        public string Username { get; set; }

        public IEnumerable<DiceRollResult> DiceRolls { get; set; }

        public IEnumerable<InitiativeRollResult> InitiativeRolls { get; set; }
    }
}
