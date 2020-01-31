using Microsoft.AspNetCore.Mvc;
using D6Roleplayer.Web.Services;
using Microsoft.AspNetCore.Http.Extensions;
using D6Roleplayer.Web.Constants;

namespace D6Roleplayer.Web.Controllers
{
    public class RoleplaySessionController : Controller
    {
        private readonly IRoleplaySessionService roleplaySessionService;

        public RoleplaySessionController(IRoleplaySessionService roleplaySessionService)
        {
            this.roleplaySessionService = roleplaySessionService;
        }

        public IActionResult Index(string sessionId, bool create)
        {
            var roleplaySession = roleplaySessionService.GetRoleplaySessionViewModel(
                sessionId, Request.Cookies[CookieConstants.UserCookie], create);

            if (roleplaySession == null)
            {
                return Redirect($"{Request.GetDisplayUrl()}?sessionId={sessionId}");
            }
            else if (roleplaySession != null)
            {
                return View("~/Views/RoleplaySession/Index.cshtml", roleplaySession);
            }

            return NotFound();
        }
    }
}