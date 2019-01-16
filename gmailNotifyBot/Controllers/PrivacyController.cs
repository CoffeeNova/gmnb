using System.Web.Mvc;

namespace CoffeeJelly.gmailNotifyBot.Controllers
{
    public class PrivacyController : Controller
    {
        public ActionResult Index()
        {
            return Redirect(@"https://telegram.org/privacy");
        }
    }
}