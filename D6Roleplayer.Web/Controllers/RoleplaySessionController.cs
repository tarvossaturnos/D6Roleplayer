using d6roleplayer.Constants;
using d6roleplayer.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace d6roleplayer.Controllers
{
    public class RoleplaySessionController : Controller
    {
        private readonly DatabaseContext databaseContext;
        const int MaxResults = 25;

        public RoleplaySessionController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public IActionResult Index(string sessionId, bool create)
        {
            var session = databaseContext.RoleplaySessions.FirstOrDefault(session => session.Id == sessionId);

            if (session == null && create)
            {
                // Create a new Roleplay Session.
                session = new RoleplaySession
                {
                    Id = sessionId
                };

                databaseContext.Add(session);
                databaseContext.SaveChanges();

                return Redirect($"{Request.GetDisplayUrl()}?sessionId={sessionId}");
            }
            else if (session != null)
            {
                string username = Request.Cookies[CookieConstants.UserCookie];
                if (string.IsNullOrWhiteSpace(username))
                {
                    username = DefaultUser.Name;
                }

                var diceRolls = databaseContext.DiceRollResults
                    .Where(result => result.RoleplaySessionId == session.Id)
                    .OrderByDescending(result => result.Id)
                    .Take(MaxResults);

                var initiativeRolls = databaseContext.InitiativeRollResults
                    .Where(result => result.RoleplaySessionId == session.Id)
                    .OrderByDescending(result => result.Roll);

                var viewModel = new DiceRollsViewModel
                {
                    Username = username,
                    RoleplaySessionId = session.Id,
                    DiceRolls = diceRolls,
                    InitiativeRolls = initiativeRolls
                };

                return View("~/Views/RoleplaySession/Index.cshtml", viewModel);
            }

            return NotFound();
        }
    }
}