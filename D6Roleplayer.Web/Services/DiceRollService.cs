using d6roleplayer.Constants;
using d6roleplayer.Models;
using D6Roleplayer.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace d6roleplayer.Services
{
    public class DiceRollService : IDiceRollService
    {
        private readonly IDiceRollRepository diceRollRepository;
        private readonly IInitiativeRollRepository initiativeRollRepository;

        public DiceRollService(
            IDiceRollRepository diceRollRepository,
            IInitiativeRollRepository initiativeRollRepository)
        {
            this.diceRollRepository = diceRollRepository;
            this.initiativeRollRepository = initiativeRollRepository;
        }

        public DiceRollResult GetDiceRollResult(DiceRollRequest request)
        {
            request = ValidateDiceRollRequest(request);

            var diceRolls = new List<int>();
            var random = new Random();

            for (int i = 0; i < request.Count; i++)
            {
                diceRolls.Add(random.Next(1, 7));
            }

            var (success, resultMessage) = CalculateDiceRollSuccess(diceRolls);

            var diceRollResult = new DiceRollResult
            {
                RoleplaySessionId = request.RoleplaySessionId,
                Username = request.Username,
                Message = request.Message,
                Rolls = string.Join(", ", diceRolls),
                ResultMessage = resultMessage,
                Success = success,
            };

            diceRollRepository.Create(diceRollResult);           

            return diceRollResult;
        }

        public InitiativeRollResult GetInitiativeRollResult(InitiativeRollRequest request)
        {
            request = ValidateInitiativeRollRequest(request);

            int roll = new Random().Next(1, 7) + request.Bonus;

            var initiativeRollResult = new InitiativeRollResult
            {
                RoleplaySessionId = request.RoleplaySessionId,
                Username = request.Username,
                Roll = roll
            };

            initiativeRollRepository.Create(initiativeRollResult);
            
            return initiativeRollResult;
        }

        public void ResetInitiativeRollResults(string sessionId)
        {
            var initiativeRolls = initiativeRollRepository.Read(sessionId);
            initiativeRollRepository.Delete(initiativeRolls);
        }

        private (bool success, string resultMessage) CalculateDiceRollSuccess(List<int> diceRolls)
        {
            bool success = false;
            string resultMessage = string.Empty;

            if (diceRolls.Any(die => die == 6))
            {
                if (diceRolls.Count(die => die == 6) >= 2 && !diceRolls.Any(die => die == 1))
                    resultMessage = "Epic Success";
                else
                    resultMessage = "Success";

                success = true;
            }
            else
            {
                if (diceRolls.Count(die => die == 1) >= 2)
                    resultMessage = "Epic fail";
                else
                    resultMessage = "Fail";
            }

            return (success, resultMessage);
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