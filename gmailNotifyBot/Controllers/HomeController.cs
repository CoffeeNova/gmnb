using System.Web.Mvc;

namespace CoffeeJelly.gmailNotifyBot.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect(@"http://t.me/lazymailbot");
        }
    }
}