using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CoffeeJelly.gmailNotifyBot.Bot;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using CoffeeJelly.gmailNotifyBot.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            Requests br = new Requests(token);
            //RequestsHandler rh = new RequestsHandler();
            //rh.TelegramTextMessageEvent += Rh_TelegramTextMessageEvent;
            Requests.RequestsArrivedEvent += BotRequests_RequestsArrivedEvent;
            
        }

        private static async void BotRequests_RequestsArrivedEvent(IRequests requests)
        {
            //Console.WriteLine(requests.LastUpdateId);
            foreach (var r in requests.RequestList)
            {
               await WriteParamsToTestFileAsync(r);
            }
        }

        private static async Task WriteParamsToTestFileAsync(JToken request)
        {

            //var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
            var path = HttpRuntime.AppDomainAppPath;
            var fileName = "testRequests.txt";


            using (var fs = new FileStream(path.PathFormatter() + fileName, FileMode.Append, FileAccess.Write))
            {
                    byte[] info = new UTF8Encoding(true).GetBytes(request + "\r\n");
                    await fs.WriteAsync(info, 0, info.Length);
            }
        }

        private const string TelegramBotThreadName = "Telegram Bot Thread";
        private RequestsHandler _BotRequestsHandler;
    }

}
