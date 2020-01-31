using System.Collections.Generic;

namespace D6Roleplayer.Infrastructure.Models
{
    public class RoleplayDiceRollResponse
    {
        public List<int> Rolls { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
