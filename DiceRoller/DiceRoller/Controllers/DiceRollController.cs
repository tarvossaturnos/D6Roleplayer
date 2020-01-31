using DiceRoller.Models;
using DiceRoller.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiceRoller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiceRollController : ControllerBase
    {
        private readonly IDiceRollService diceRollService;

        public DiceRollController(IDiceRollService diceRollService)
        {
            this.diceRollService = diceRollService;
        }

        [HttpGet]
        public string Roll()
        {
            return diceRollService.RollDice().ToString();
        }

        [HttpGet]
        [Route("RollRoleplayDices")]
        public RoleplayDiceRollResponse RollRoleplayDices(int amount)
        {
            return diceRollService.GetRoleplayDiceResult(amount);
        }
    }
}
