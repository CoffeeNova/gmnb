using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CoffeeJelly.gmailNotifyBot.Bot;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace CoffeeJelly.gmailNotifyBot
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected async void Application_Start()
        {
            Database.SetInitializer(new UserDbInitializer());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var botSettings = BotSettings.GetInstance();
#pragma warning disable 618
            botSettings.Username = System.Configuration.ConfigurationSettings.AppSettings["Username"];
#pragma warning restore 618

            LogMaker.NewMessage += LogMaker_NewMessage;
            string botToken = App_LocalResources.Tokens.GmailControlBotToken;
            var topicName = App_LocalResources.Tokens.TopicName;

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
            _authorizer = Authorizer.GetInstance(botToken, _updatesHandler, clientSecret);

            //generate ServiceCollection for all gmail control bot users
            var gmailServiceFactory = ServiceFactory.GetInstanse(clientSecret);
            await gmailServiceFactory.RestoreServicesFromStore();

            _commandHandler = CommandHandler.GetInstance(botToken, _updatesHandler, clientSecret, topicName);

            var botServices = gmailServiceFactory.ServiceCollection;
            //restart push notification watches for all gmail control bot users
            botServices.ForEach(async s =>
            {
                await _commandHandler.HandleStartWatchCommand(s);
                //probably i need to do a delay here to avoid response ddos to my server
            });
            //start timer which would be update push notification watch for users which expiration time approaches the end
            _pushNotificationWatchTimer = new Timer(state =>
            {
                var userSettings = gmailDbContextWorker.GetAllUsersSettings();
                userSettings.ForEach(us =>
                {
                    var difference = DateTime.UtcNow.Difference(us.Expiration);
                    if (difference.TotalHours >= 2) return;
                    var service = botServices.FirstOrDefault(s => s.From == us.UserId);
                    if (service == null) return;
                    _commandHandler.HandleStartWatchCommand(service).Wait();
                });
            }, null, _updatePeriod, _updatePeriod);
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
        private CommandHandler _commandHandler;
        private Timer _pushNotificationWatchTimer;
        private int _updatePeriod = 3600000; // 1 hour
        private GmailDbContextWorker gmailDbContextWorker;
    }

}
