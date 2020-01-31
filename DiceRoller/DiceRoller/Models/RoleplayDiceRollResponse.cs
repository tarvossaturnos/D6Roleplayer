using System.Collections.Generic;

namespace DiceRoller.Models
{
    public class RoleplayDiceRollResponse
    {
        public List<int> Rolls { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
