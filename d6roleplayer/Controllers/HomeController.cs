using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using d6roleplayer.Models;
using System;
using System.Text.RegularExpressions;

namespace d6roleplayer.Controllers
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
            return $"{GetRandomNumber()}{GetRandomNumber()}{GetRandomNumber()}{GetRandomNumber()}{GetRandomNumber()}{GetRandomNumber()}";
        }

        private static string GetRandomNumber()
        {
            return new Random().Next(1, 10).ToString();
        }
    }
}