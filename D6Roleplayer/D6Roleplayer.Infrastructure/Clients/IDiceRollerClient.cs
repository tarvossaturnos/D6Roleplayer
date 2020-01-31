using System.Threading.Tasks;

namespace D6Roleplayer.Infrastructure.Clients
{
    public interface IDiceRollerClient
    {
        Task<string> ProcessDiceRollRequest();

        Task<string> ProcessRollRoleplayDicesRequest(int amount);
    }
}
