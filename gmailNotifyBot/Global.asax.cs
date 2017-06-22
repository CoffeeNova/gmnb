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
using System.Net;
using System.Reflection;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public void Session_Start(Object source, EventArgs e)
        {
            
        }


        protected void Application_Start()
        {
            Database.SetInitializer(new UserDbInitializer());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            LogMaker.NewMessage += LogMaker_NewMessage;

#pragma warning disable 618
#if DEBUG
            string clientSecretsStr = Encoding.UTF8.GetString(App_LocalResources.Tokens.client_secret_debug);
#else
            string clientSecretsStr = Encoding.UTF8.GetString(App_LocalResources.Tokens.client_secret);
#endif
 
            var clienSecretsJtoken = JsonConvert.DeserializeObject<JToken>(clientSecretsStr);
            var clientSecrets = JsonConvert.DeserializeObject<Secrets>(clienSecretsJtoken["web"].ToString());
            var botSettings = new BotSettings
            {
                BotName = System.Configuration.ConfigurationSettings.AppSettings["Botname"],
                Token = App_LocalResources.Tokens.GmailControlBotToken,
                Topic = App_LocalResources.Tokens.TopicName,
                ClientSecrets = clientSecrets,
                Subscription = App_LocalResources.Tokens.Subscription,
                ImagesPath = System.Configuration.ConfigurationSettings.AppSettings["ImagesPath"],
                DomainName = App_LocalResources.Tokens.DomainName,
                AttachmentsTempFolder = Path.Combine(HttpRuntime.AppDomainAppPath, System.Configuration.ConfigurationSettings.AppSettings["AttachmentsTemp"]),
                MaxAttachmentSize = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["MaxAttachmentSize"]),
                BotVersion = ReturnBotVersion(),
                GmnbApiKey = App_LocalResources.Tokens.gmnbAPIKey,
                ApplicationName = App_LocalResources.Tokens.ApplicationName
            };
            _botInitializer = BotInitializer.GetInstance(botSettings);
            _botInitializer.InitializeUpdates(true);
            _botInitializer.InitializeUpdatesHandler();
            _botInitializer.InitializeAuthotizer();
            _botInitializer.InitializeServiceFactory();
            _botInitializer.InitializeMessageHandler();
            _botInitializer.InitializeCallbackQueryHandler();
            _botInitializer.InitializeInlineQueryHandler();
            _botInitializer.InitializeChosenInlineResultHandler();
#if !DEBUG
            _botInitializer.InitializeNotifyHandler();
            _botInitializer.InitializePushNotificationWatchesAsync(initializeDelay);
            _botInitializer.InitializePushNotificationWatchTimer(_updatePeriod);
#endif
#pragma warning restore 618
        }

        private void LogMaker_NewMessage(NLog.Logger logger, string message, DateTime time, bool isError, string stackTrace)
        {
            Debug.WriteLine($"{logger.Name} log message: {message}");
#if !DEBUG
            TestModel.WriteLogToFile($"{time}   {logger.Name} log message: {message}{Environment.NewLine}{stackTrace}{Environment.NewLine}");
            if (stackTrace != null)
                TestModel.WriteLogToFile($"{stackTrace}{Environment.NewLine}");
#endif
        }

        private string ReturnBotVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        private int _updatePeriod = 3600000; // 1 hour
        private int initializeDelay = 20000; //20 sec
        private BotInitializer _botInitializer;
    }

}
