using D6Roleplayer.Models;

namespace D6Roleplayer.Infrastructure.Repositories
{
    public interface IRoleplaySessionRepository
    {
        void Create(RoleplaySession roleplaySession);

        RoleplaySession Read(string sessionId);
    }
}
