using D6Roleplayer.Infrastructure.Models;

namespace D6Roleplayer.Infrastructure.Repositories
{
    public interface IRoleplaySessionRepository
    {
        void Create(RoleplaySession roleplaySession);

        RoleplaySession Read(string sessionId);
    }
}
