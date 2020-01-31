using DiceRoller.Models;

namespace DiceRoller.Services
{
    public interface IDiceRollService
    {
        int RollDice();

        RoleplayDiceRollResponse GetRoleplayDiceResult(int amount);
    }
}
