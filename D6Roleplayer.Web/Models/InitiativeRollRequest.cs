namespace D6Roleplayer.Web.Models
{
    public class InitiativeRollRequest
    {
        public string Username { get; set; }
        public int Bonus { get; set; }
        public string RoleplaySessionId { get; set; }
    }
}