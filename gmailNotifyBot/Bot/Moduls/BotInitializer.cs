using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.Handler;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.Handler.Message;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using Google.Apis.Auth.OAuth2;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls
{
    public sealed class BotInitializer
    {
        private BotInitializer(BotSettings botSettings)
        {
            Debug.Assert(botSettings.AllSettingsAreSet(), "Set all properties at BotSettings class.");
            BotSettings = botSettings;
        }

        public static BotInitializer GetInstance(BotSettings botSettings)
        {
            if (Instance != null)
                return Instance;

            lock (Locker)
            {
                Instance = new BotInitializer(botSettings);
            }
            return Instance;
        }

        public void InitializeUpdates()
        {
            if (string.IsNullOrEmpty(BotSettings.Token))
                throw new InvalidOperationException($"{nameof(BotSettings.Token)} property must be specified");
            Updates = Updates.GetInstance(BotSettings.Token);
            Updates.UpdatesTracingStoppedEvent += Updates_UpdatesTracingStoppedEvent;
        }

        public void InitializeUpdatesHandler()
        {
            UpdatesHandler = new UpdatesHandler();
        }

        public void InitializeAuthotizer()
        {
            if (string.IsNullOrEmpty(BotSettings.Token))
                throw new InvalidOperationException($"{nameof(BotSettings.Token)} property must be specified");
            if (UpdatesHandler == null)
                throw new InvalidOperationException($"{nameof(UpdatesHandler)} property must be initialized first.");
            if (BotSettings.ClientSecrets == null)
                throw new InvalidOperationException($"{nameof(BotSettings.ClientSecrets)} property must be specified");
            Authorizer = Authorizer.GetInstance(BotSettings.Token, UpdatesHandler, BotSettings.ClientSecrets);
        }

        public void InitializeServiceFactory()
        {
            if (BotSettings.ClientSecrets == null)
                throw new InvalidOperationException($"{nameof(BotSettings.ClientSecrets)} property must be specified");
            ServiceFactory = ServiceFactory.GetInstanse(BotSettings.ClientSecrets);
            //generate ServiceCollection for all gmail control bot users
            ServiceFactory.RestoreServicesFromRepository();
        }

        public void InitializeCommandHandler()
        {
            if (string.IsNullOrEmpty(BotSettings.Token))
                throw new InvalidOperationException($"{nameof(BotSettings.Token)} property must be specified");
            if (UpdatesHandler == null)
                throw new InvalidOperationException($"{nameof(UpdatesHandler)} property must be initialized first.");
            if (BotSettings.ClientSecrets == null)
                throw new InvalidOperationException($"{nameof(BotSettings.ClientSecrets)} property must be specified");
            if (string.IsNullOrEmpty(BotSettings.Topic))
                throw new InvalidOperationException($"{nameof(BotSettings.Topic)} property must be specified");

            CommandHandler = CommandHandler.GetInstance(BotSettings.Token, UpdatesHandler, BotSettings.ClientSecrets, BotSettings.Topic);
        }

        public void InitializeMessageHandler()
        {
            MessageHandler = new MessageHandler();
        }

        //restart push notification watches for all gmail control bot users
        public void InitializePushNotificationWatchesAsync(int delay)
        {
            Task.Run(() =>
            {
                Task.Delay(delay).Wait();
                ServiceFactory?.ServiceCollection.ForEach(async s =>
                {
                    await CommandHandler.HandleStartWatchCommandAsync(s);
                    //probably i need to do a delay here to avoid response ddos to my server
                });
            });
        }

        public void InitializePushNotificationWatchTimer(int updatePeriod)
        {
            if (_gmailDbContextWorker == null)
                _gmailDbContextWorker = new GmailDbContextWorker();
            //start timer which would be update push notification watch for users which expiration time approaches the end
            PushNotificationWatchTimer = new Timer(state =>
            {
                var userSettings = _gmailDbContextWorker.GetAllUsersSettings();
                userSettings.ForEach(us =>
                {
                    var difference = DateTime.UtcNow.Difference(us.Expiration);
                    if (difference.TotalHours >= 2) return;
                    var service = ServiceFactory?.ServiceCollection.FirstOrDefault(s => s.From == us.UserId);
                    if (service == null) return;
                    CommandHandler?.HandleStartWatchCommand(service);
                });
            }, null, updatePeriod, updatePeriod);
        }

        private async void Updates_UpdatesTracingStoppedEvent(object sender, BotRequestErrorEventArgs e)
        {
            await CoffeeJTools.Delay(5000);
            Updates.Restart();
        }



        public static BotInitializer Instance { get; private set; }

        public BotSettings BotSettings { get; set; }

        public Updates Updates { get; private set; }

        public UpdatesHandler UpdatesHandler { get; private set; }

        public Authorizer Authorizer { get; set; }

        public ServiceFactory ServiceFactory { get; set; }

        public CommandHandler CommandHandler { get; set; }

        public MessageHandler MessageHandler { get; set; }

        public Timer PushNotificationWatchTimer { get; private set; }

        private static readonly object Locker = new object();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private GmailDbContextWorker _gmailDbContextWorker;
    }
}