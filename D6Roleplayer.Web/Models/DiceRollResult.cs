using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace d6roleplayer.Models
{
    public class DiceRollResult
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("RoleplaySession")]
        public string RoleplaySessionId { get; set; }

        public string Rolls { get; set; }

        public string Message { get; set; }

        public string Username { get; set; }

        public string ResultMessage { get; set; }

        public bool Success { get; set; }
    }
}