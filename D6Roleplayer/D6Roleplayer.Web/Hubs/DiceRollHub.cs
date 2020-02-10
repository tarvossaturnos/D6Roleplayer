using D6Roleplayer.Web.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using D6Roleplayer.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace D6Roleplayer.Web.Hubs
{
    public class DiceRollHub : Hub
    {
        private readonly IDiceRollService diceRollService;

        private static List<string> connections = new List<string>();

        public DiceRollHub(IDiceRollService diceRollService)
        {
            this.diceRollService = diceRollService;
        }

        public override async Task OnConnectedAsync()
        {
            var roleplaySessionId = GetRoleplaySessionId();
            await Groups.AddToGroupAsync(Context.ConnectionId, roleplaySessionId);
            connections.Add(roleplaySessionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var roleplaySessionId = GetRoleplaySessionId();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roleplaySessionId);
            connections.Remove(roleplaySessionId);

            await base.OnDisconnectedAsync(exception);
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

        public async Task UpdateUserCount()
        {
            var roleplaySessionId = GetRoleplaySessionId();
            await Clients.Group(GetRoleplaySessionId()).SendAsync("UpdateUserCount",
                connections.Where(connection => connection == roleplaySessionId).Count());
        }

        private string GetRoleplaySessionId()
        {
            var httpContext = Context.GetHttpContext();
            return httpContext.Request.Query["sessionId"];
        }
    }
}