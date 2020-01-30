using System;
using Microsoft.AspNetCore.Mvc;

namespace DiceRoller.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiceRollController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            var random = new Random();
            return random.Next(1, 7).ToString();
        }
    }
}
