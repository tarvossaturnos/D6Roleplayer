using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace D6Roleplayer.Infrastructure.Models
{
    public class InitiativeRollResult
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("RoleplaySession")]
        public string RoleplaySessionId { get; set; }
        public string Username { get; set; }
        public int Roll { get; set; }
    }
}