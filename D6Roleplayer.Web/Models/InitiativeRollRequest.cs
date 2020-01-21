namespace D6Roleplayer.Models
{
    public class InitiativeRollRequest
    {
        public string Username { get; set; }
        public int Bonus { get; set; }
        public string RoleplaySessionId { get; set; }
    }
}