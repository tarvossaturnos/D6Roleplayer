using d6roleplayer.Models;
using System.Threading.Tasks;

namespace D6Roleplayer.Infrastructure.Repositories
{
    public interface IRoleplaySessionRepository
    {
        Task Create(RoleplaySession roleplaySession);

        Task<RoleplaySession> Read(string sessionId);
    }
}
