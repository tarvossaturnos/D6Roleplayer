using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace D6Roleplayer.Infrastructure.Clients
{
    public class DiceRollerClient : IDiceRollerClient
    {
        private static HttpClient Client = new HttpClient();

        public string ProcessDiceRollRequest()
        {

        }
    }
}
