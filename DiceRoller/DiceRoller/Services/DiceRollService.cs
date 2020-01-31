using DiceRoller.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceRoller.Services
{
    public class DiceRollService : IDiceRollService
    {
        public int RollDice()
        {
            var random = new Random();

            return random.Next(1, 7);
        }

        public RoleplayDiceRollResponse GetRoleplayDiceResult(int amount)
        {
            var response = new RoleplayDiceRollResponse
            {
                Rolls = new List<int>()
            };

            for (int i = 0; i < amount; i++)
            {
                response.Rolls.Add(RollDice());
            }

            if (response.Rolls.Any(die => die == 6))
            {
                if (response.Rolls.Count(die => die == 6) >= 2 && !response.Rolls.Any(die => die == 1))
                    response.Message = "Epic Success";
                else
                    response.Message = "Success";

                response.Success = true;
            }
            else
            {
                if (response.Rolls.Count(die => die == 1) >= 2)
                    response.Message = "Epic fail";
                else
                    response.Message = "Fail";
            }

            return response;
        }
    }
}
