using D6Roleplayer.Web.Constants;
using D6Roleplayer.Infrastructure.Repositories;
using D6Roleplayer.Web.Models;
using D6Roleplayer.Infrastructure.Models;

namespace D6Roleplayer.Web.Services
{
    public class RoleplaySessionService : IRoleplaySessionService
    {
        private readonly IRoleplaySessionRepository roleplaySessionRepository;
        private readonly IDiceRollRepository diceRollRepository;
        private readonly IInitiativeRollRepository initiativeRollRepository;

        public RoleplaySessionService(
            IRoleplaySessionRepository roleplaySessionRepository, 
            IDiceRollRepository diceRollRepository, 
            IInitiativeRollRepository initiativeRollRepository)
        {
            this.roleplaySessionRepository = roleplaySessionRepository;
            this.diceRollRepository = diceRollRepository;
            this.initiativeRollRepository = initiativeRollRepository;
        }
        
        public RoleplaySessionViewModel GetRoleplaySessionViewModel(string sessionId, string username, bool create)
        {
            var session = roleplaySessionRepository.Read(sessionId);

            if (session == null && create)
            {
                session = new RoleplaySession { Id = sessionId };
                roleplaySessionRepository.Create(session);

                return null;
            }
            else if (session != null)
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    username = DefaultUser.Name;
                }

                var diceRolls = diceRollRepository.Read(session.Id);
                var initiativeRolls = initiativeRollRepository.Read(session.Id);

                return new RoleplaySessionViewModel
                {
                    Username = username,
                    RoleplaySessionId = session.Id,
                    DiceRolls = diceRolls,
                    InitiativeRolls = initiativeRolls
                };
            }

            return null;
        }
    }
}
