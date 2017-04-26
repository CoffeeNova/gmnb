using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CoffeeJelly.gmailNotifyBot.Bot;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using CoffeeJelly.gmailNotifyBot.Extensions;
using CoffeeJelly.gmailNotifyBot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CoffeeJelly.gmailNotifyBot
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           // Database.SetInitializer(new UserDbInitializer());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            LogMaker.NewMessage += LogMaker_NewMessage;
            string botToken = App_LocalResources.Tokens.GmailControlBotToken;
#if DEBUG
            string clientSecretStr = Encoding.UTF8.GetString(App_LocalResources.Tokens.client_secret_debug);
#else
            string clientSecretStr = Encoding.UTF8.GetString(App_LocalResources.Tokens.client_secret);
#endif

            var clienSecretJtoken = JsonConvert.DeserializeObject<JToken>(clientSecretStr);
            var clientSecret = JsonConvert.DeserializeObject<Secrets>(clienSecretJtoken["web"].ToString());

            
            _updates = Updates.GetInstance(botToken);
            _updates.UpdatesTracingStoppedEvent += Updates_UpdatesTracingStoppedEvent;
            _updatesHandler = new UpdatesHandler();
            _authorizer = Authorizer.GetInstance(botToken, _updatesHandler, clientSecret, scopes);
            var test = new GmailServiceFactory(clientSecret, scopes);

        }

        private void LogMaker_NewMessage(NLog.Logger logger, string message, DateTime time, bool isError)
        {
            Debug.WriteLine($"{logger.Name} log message: {message}");
        }

        private async void Updates_UpdatesTracingStoppedEvent(object sender, BotRequestErrorEventArgs e)
        {
            await CoffeeJTools.Delay(5000);
            _updates.Restart();
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
        private UpdatesHandler _updatesHandler;
        private Updates _updates;
        private Authorizer _authorizer;
    }

}
