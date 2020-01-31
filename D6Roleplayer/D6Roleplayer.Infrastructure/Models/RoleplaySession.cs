using System.ComponentModel.DataAnnotations;

namespace D6Roleplayer.Infrastructure.Models
{
    public class RoleplaySession
    {
        [Key]
        public string Id { get; set; }
    }
}