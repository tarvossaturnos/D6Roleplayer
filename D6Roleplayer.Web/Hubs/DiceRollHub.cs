using d6roleplayer.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using d6roleplayer.Services;

namespace d6roleplayer.Hubs
{
    public class DiceRollHub : Hub
    {
        private readonly IDiceRollService diceRollService;

        public DiceRollHub(IDiceRollService diceRollService)
        {
            this.diceRollService = diceRollService;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetRoleplaySessionId());
            await base.OnConnectedAsync();
        }

        public async Task RequestDiceRoll(string username, string message, string count)
        {
            var diceRollRequest = new DiceRollRequest
            {
                Username = username,
                Message = message,
                Count = int.Parse(count),
                RoleplaySessionId = GetRoleplaySessionId()
            };

            var result = diceRollService.GetDiceRollResult(diceRollRequest);            

            await Clients.Group(result.RoleplaySessionId).SendAsync(
                "UpdateDiceRolls",
                result.Username,
                result.Message,
                result.Rolls,
                result.ResultMessage,
                result.Success);
        }

        public async Task RequestInitiativeRoll(string username, string bonus)
        {
            var initiativeRoll = new InitiativeRollRequest
            {
                Username = username,
                Bonus = int.Parse(bonus),
                RoleplaySessionId = GetRoleplaySessionId()
            };

            var result = diceRollService.GetInitiativeRollResult(initiativeRoll);

            await Clients.Group(GetRoleplaySessionId()).SendAsync(
                "UpdateInitiativeRolls",
               result.Username,
               result.Roll);
        }

        public async Task RequestResetInitiativeRolls()
        {
            diceRollService.ResetInitiativeRollResults(GetRoleplaySessionId());

            await Clients.Group(GetRoleplaySessionId()).SendAsync("ResetInitiativeRolls");
        }

        public async Task RequestDrawingClick(int[] x, int[] y, string[] color, bool[] drag)
        {
            await Clients.Group(GetRoleplaySessionId()).SendAsync("UpdateDrawing", x, y, color, drag);
        }

        public async Task RequestSyncDrawing(int[] x, int[] y, string[] color, bool[] drag)
        {
            await Clients.Group(GetRoleplaySessionId()).SendAsync("SyncDrawing", x, y, color, drag);
        }

        public async Task RequestResetDrawing()
        {
            await Clients.Group(GetRoleplaySessionId()).SendAsync("ResetDrawing");
        }

        private string GetRoleplaySessionId()
        {
            var httpContext = Context.GetHttpContext();
            return httpContext.Request.Query["sessionId"];
        }
    }
}