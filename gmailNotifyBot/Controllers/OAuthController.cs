using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CoffeeJelly.gmailNotifyBot.Bot;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls;

namespace CoffeeJelly.gmailNotifyBot.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return null;
        }

        [HttpGet]
        public ActionResult Code(string code, string state, string error)
        {
            Authorizer.HandleAuthResponse(code, state, error);
            return null;
        }

        [HttpGet]
        public ActionResult Token(string code, string state, string error)
        {
            return null;
        }


    }
}