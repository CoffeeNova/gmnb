using System.Web.Mvc;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;

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