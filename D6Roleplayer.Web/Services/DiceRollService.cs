﻿using d6roleplayer.Constants;
using d6roleplayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace d6roleplayer.Services
{
    public class DiceRollService : IDiceRollService
    {
        private readonly DatabaseContext databaseContext;

        public DiceRollService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
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

            databaseContext.Add(diceRollResult);
            RemoveDiceEntriesAtLimit(diceRollResult.RoleplaySessionId, 24);            

            databaseContext.SaveChanges();

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

            databaseContext.Add(initiativeRollResult);
            databaseContext.SaveChanges();

            return initiativeRollResult;
        }

        public void ResetInitiativeRollResults(string sessionId)
        {
            databaseContext.InitiativeRollResults
                .RemoveRange(databaseContext.InitiativeRollResults
                .Where(result => result.RoleplaySessionId == sessionId));
            databaseContext.SaveChanges();
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

        private void RemoveDiceEntriesAtLimit(string sessionId, int limit)
        {
            var entriesToDelete = databaseContext.DiceRollResults
              .Where(diceRoll => diceRoll.RoleplaySessionId == sessionId)
              .OrderByDescending(result => result.Id)
              .Skip(limit);

            foreach (var entry in entriesToDelete)
            {
                databaseContext.Remove(entry);
            }
        }
    }
}