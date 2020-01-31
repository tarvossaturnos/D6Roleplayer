namespace D6Roleplayer.Web.Models
{
    public class DiceRollRequest
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
        public string RoleplaySessionId { get; set; }
    }
}