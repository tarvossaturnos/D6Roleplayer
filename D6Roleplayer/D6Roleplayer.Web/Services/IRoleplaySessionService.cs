using D6Roleplayer.Web.Models;

namespace D6Roleplayer.Web.Services
{
    public interface IRoleplaySessionService
    {
        public RoleplaySessionViewModel GetRoleplaySessionViewModel(string sessionId, string username, bool create);
    }
}
