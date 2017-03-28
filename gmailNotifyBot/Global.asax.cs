using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CoffeeJelly.gmailNotifyBot.Bot;

namespace CoffeeJelly.gmailNotifyBot
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //ThreadPool.QueueUserWorkItem((state) => { Thread.CurrentThread.Name = TelegramBotThreadName; RunTelegramBot(); });
            _gmnbRequestsHandler = new RequestsHandler
            {
                HandleConnectCommand = true
            };
        }

        private static void RunTelegramBot()
        {
            //FOR TEST ONLY! NOT SECURE!
            string token = "252886092:AAHxtq8ZINX6WJXcT-MuQFoarH9-8Ppntl8";

            
        }

        private static void GmnbRequests_RequestsArrivedEvent(IRequests requests)
        {
            //Console.WriteLine(requests.LastUpdateId);
            foreach (var r in requests.Requests)
            {
                Console.WriteLine(r);
            }
            var dr = new DateTime();
        }

        private const string TelegramBotThreadName = "Telegram Bot Thread";
        private RequestsHandler _gmnbRequestsHandler;
    }

}
