using D6Roleplayer.Web.Constants;
using D6Roleplayer.Web.Models;
using D6Roleplayer.Infrastructure.Repositories;
using D6Roleplayer.Infrastructure.Models;
using D6Roleplayer.Infrastructure.Clients;
using Newtonsoft.Json;

namespace D6Roleplayer.Web.Services
{
    public class DiceRollService : IDiceRollService
    {
        private readonly IDiceRollRepository diceRollRepository;
        private readonly IInitiativeRollRepository initiativeRollRepository;
        private readonly IDiceRollerClient diceRollClient;

        public DiceRollService(
            IDiceRollRepository diceRollRepository,
            IInitiativeRollRepository initiativeRollRepository,
            IDiceRollerClient diceRollClient)
        {
            this.diceRollRepository = diceRollRepository;
            this.initiativeRollRepository = initiativeRollRepository;
            this.diceRollClient = diceRollClient;
        }

        public DiceRollResult GetDiceRollResult(DiceRollRequest request)
        {
            request = ValidateDiceRollRequest(request);

            var roleplayDiceRollResponse = JsonConvert
                .DeserializeObject<RoleplayDiceRollResponse>(diceRollClient.ProcessRollRoleplayDicesRequest(request.Count).Result);

            var diceRollResult = new DiceRollResult
            {
                RoleplaySessionId = request.RoleplaySessionId,
                Username = request.Username,
                Message = request.Message,
                Rolls = string.Join(", ", roleplayDiceRollResponse.Rolls),
                ResultMessage = roleplayDiceRollResponse.Message,
                Success = roleplayDiceRollResponse.Success,
            };

            diceRollRepository.Create(diceRollResult);           

            return diceRollResult;
        }

        public InitiativeRollResult GetInitiativeRollResult(InitiativeRollRequest request)
        {
            request = ValidateInitiativeRollRequest(request);
            int.TryParse(diceRollClient.ProcessDiceRollRequest().Result, out int roll);

            var initiativeRollResult = new InitiativeRollResult
            {
                RoleplaySessionId = request.RoleplaySessionId,
                Username = request.Username,
                Roll = roll + request.Bonus
            };

            initiativeRollRepository.Create(initiativeRollResult);
            
            return initiativeRollResult;
        }

        public void ResetInitiativeRollResults(string sessionId)
        {
            var initiativeRolls = initiativeRollRepository.Read(sessionId);
            initiativeRollRepository.Delete(initiativeRolls);
        }

        private DiceRollRequest ValidateDiceRollRequest(DiceRollRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                request.Username = DefaultUser.Name;

            if (string.IsNullOrWhiteSpace(request.Message))
                request.Message = "Dice Roll";

            if (request.Count > 12)
                request.Count = 12;
            else if (request.Count < 1)
                request.Count = 1;

            return request;
        }

        private InitiativeRollRequest ValidateInitiativeRollRequest(InitiativeRollRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                request.Username = DefaultUser.Name;

            if (request.Bonus > 10)
                request.Bonus = 10;
            else if (request.Bonus < 0)
                request.Bonus = 1;

            return request;
        }
    }
}