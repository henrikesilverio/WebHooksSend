﻿using System.Threading.Tasks;
using System.Web.Mvc;

namespace CustomWebHooksSender.Controllers
{
    [Authorize]
    public class NotifyController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Submit()
        {
            // Create an event with action 'event1' and additional data
            await this.NotifyAsync("event1", new { P1 = "p1" });

            return new EmptyResult();
        }
    }
}