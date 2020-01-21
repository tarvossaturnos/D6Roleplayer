using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using D6Roleplayer.Web.Models;
using System;

namespace D6Roleplayer.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                RoleplaySessionId = GenerateRoleplaySessionId()
            };

            return View("~/Views/Home/Index.cshtml", viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static string GenerateRoleplaySessionId()
        {
            return new Random().Next(1000, 10000).ToString();
        }
    }
}