using System;
using System.Net;
using System.Web.Mvc;

namespace CoffeeJelly.gmailNotifyBot.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public ActionResult Index(string state, string code)
        {

            var uri = new Uri($"https://telegram.me/GmailNotifyBot?start=state{state}code={code}");
            var request = WebRequest.Create(uri);

            request.GetResponse();
            return null;
        }
    }
}