using System;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CoffeeJelly.gmailNotifyBot.Bot;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;

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
            //_BotRequestsHandler = new RequestsHandler
            //{
            //    HandleConnectCommand = true
            //};
            string token = App_LocalResources.Tokens.GmailControlBotToken;
            BotRequests br = new BotRequests(token);
            //RequestsHandler rh = new RequestsHandler();
            //rh.TelegramTextMessageEvent += Rh_TelegramTextMessageEvent;
            BotRequests.RequestsArrivedEvent += BotRequests_RequestsArrivedEvent;
            
        }

        private static void BotRequests_RequestsArrivedEvent(IRequests requests)
        {
            //Console.WriteLine(requests.LastUpdateId);
            foreach (var r in requests.Requests)
            {
                Debug.WriteLine(r);
            }
        }

        private const string TelegramBotThreadName = "Telegram Bot Thread";
        private RequestsHandler _BotRequestsHandler;
    }

}
