using D6Roleplayer.Constants;
using D6Roleplayer.Models;
using D6Roleplayer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace D6Roleplayer.Controllers
{
    public class RoleplaySessionController : Controller
    {
        private readonly IRoleplaySessionRepository roleplaySessionRepository;
        private readonly IDiceRollRepository diceRollRepository;
        private readonly IInitiativeRollRepository initiativeRollRepository;

        public RoleplaySessionController(
            IRoleplaySessionRepository roleplaySessionRepository,
            IDiceRollRepository diceRollRepository,
            IInitiativeRollRepository initiativeRollRepository)
        {
            this.roleplaySessionRepository = roleplaySessionRepository;
            this.diceRollRepository = diceRollRepository;
            this.initiativeRollRepository = initiativeRollRepository;
        }

        public IActionResult Index(string sessionId, bool create)
        {
            var session = roleplaySessionRepository.Read(sessionId);

            if (session == null && create)
            {
                session = new RoleplaySession { Id = sessionId };
                roleplaySessionRepository.Create(session);

                return Redirect($"{Request.GetDisplayUrl()}?sessionId={sessionId}");
            }
            else if (session != null)
            {
                string username = Request.Cookies[CookieConstants.UserCookie];
                if (string.IsNullOrWhiteSpace(username))
                {
                    username = DefaultUser.Name;
                }

                var diceRolls = diceRollRepository.Read(session.Id);
                var initiativeRolls = initiativeRollRepository.Read(session.Id);

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