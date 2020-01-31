using D6Roleplayer.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace D6Roleplayer.Infrastructure.Clients
{
    public class DiceRollerClient : IDiceRollerClient
    {
        private static HttpClient Client = new HttpClient();
        private readonly DiceRollerSettings diceRollerSettings;

        public DiceRollerClient(IOptions<DiceRollerSettings> diceRollerSettings)
        {
            this.diceRollerSettings = diceRollerSettings.Value;
        }

        public async Task<string> ProcessDiceRollRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, diceRollerSettings.Url);
            var result = await Client.SendAsync(request).Result.Content.ReadAsStringAsync();

            return result;
        }

        public async Task<string> ProcessRollRoleplayDicesRequest(int amount)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{diceRollerSettings.Url}/RollRoleplayDices?amount={amount}");
            var response = await Client.SendAsync(request).Result.Content.ReadAsStringAsync();

            return response;
        }
    }
}
