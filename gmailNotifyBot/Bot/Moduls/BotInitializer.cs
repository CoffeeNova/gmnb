using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper;
using Google.Apis.Auth.OAuth2;
using NLog;
using CallbackQueryHandler = CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQueryUpdates.CallbackQueryHandler;
using ChosenInlineResultHandler = CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ChosenInlineResultUpdates.ChosenInlineResultHandler;
using InlineQueryHandler = CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.InlineQueryUpdates.InlineQueryHandler;
using MessageHandler = CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates.MessageHandler;

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

        public void InitializeUpdates(bool webhook = false)
        {
            if (string.IsNullOrEmpty(BotSettings.Token))
                throw new InvalidOperationException($"{nameof(BotSettings.Token)} property must be specified");
            if (webhook)
            {
                Updates = new WebhookUpdates(BotSettings.Token);
                var whook = (WebhookUpdates) Updates;
                whook.Url = $@"https://{BotSettings.DomainName}/Push/TelegramPath";
                whook.AllowedUpdates = new List<TelegramBotApiWrapper.Types.UpdateType>
                {
                    TelegramBotApiWrapper.Types.UpdateType.AllUpdates
                };
                whook.SetWebhook();
                return;
            }

            Updates = new LongPollingUpdates(BotSettings.Token);
            var lPoll = (LongPollingUpdates) Updates;
            lPoll.DeleteWebhook();
            lPoll.UpdatesTracingStoppedEvent += Updates_UpdatesTracingStoppedEvent;
            lPoll.Start();
        }

        public void InitializeUpdatesHandler()
        {
            if (Updates == null)
                throw new InvalidOperationException($"{nameof(Updates)} property must be specified");
            UpdatesHandler = new UpdatesHandler {Updates = Updates};
            UpdatesHandler.StartHandleUpdates();
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

        public void InitializeMessageHandler()
        {
            if (string.IsNullOrEmpty(BotSettings.Token))
                throw new InvalidOperationException($"{nameof(BotSettings.Token)} property must be specified");
            if (UpdatesHandler == null)
                throw new InvalidOperationException($"{nameof(UpdatesHandler)} property must be initialized first.");
            MessageHandler = new MessageHandler();
        }

        public void InitializeCallbackQueryHandler()
        {
            if (string.IsNullOrEmpty(BotSettings.Token))
                throw new InvalidOperationException($"{nameof(BotSettings.Token)} property must be specified");
            if (UpdatesHandler == null)
                throw new InvalidOperationException($"{nameof(UpdatesHandler)} property must be initialized first.");
            CallbackQueryHandler = new CallbackQueryHandler();
        }

        public void InitializeInlineQueryHandler()
        {
            if (string.IsNullOrEmpty(BotSettings.Token))
                throw new InvalidOperationException($"{nameof(BotSettings.Token)} property must be specified");
            if (UpdatesHandler == null)
                throw new InvalidOperationException($"{nameof(UpdatesHandler)} property must be initialized first.");
            InlineQueryHandler = new InlineQueryHandler();
        }

        public void InitializeChosenInlineResultHandler()
        {
            if (string.IsNullOrEmpty(BotSettings.Token))
                throw new InvalidOperationException($"{nameof(BotSettings.Token)} property must be specified");
            if (UpdatesHandler == null)
                throw new InvalidOperationException($"{nameof(UpdatesHandler)} property must be initialized first.");
            ChosenInlineResultHandler = new ChosenInlineResultHandler();
        }

        public void InitializeNotifyHandler()
        {
            if (string.IsNullOrEmpty(BotSettings.Token))
                throw new InvalidOperationException($"{nameof(BotSettings.Token)} property must be specified");
            if (UpdatesHandler == null)
                throw new InvalidOperationException($"{nameof(UpdatesHandler)} property must be initialized first.");
            if (Authorizer == null)
                throw new InvalidOperationException($"{nameof(Authorizer)} property must be initialized first.");

            NotifyHandler = new NotifyHandler();
        }

        //restart push notification watches for all gmail control bot users
        public void InitializePushNotificationWatchesAsync(int delay)
        {
            if (UpdatesHandler == null)
                throw new InvalidOperationException($"{nameof(ServiceFactory)} property must be initialized first.");
            if (UpdatesHandler == null)
                throw new InvalidOperationException($"{nameof(MessageHandler)} property must be initialized first.");
            Task.Run(() =>
            {
                Task.Delay(delay).Wait();
                ServiceFactory?.ServiceCollection.ForEach(async s =>
                {
                    await MessageHandler.HandleStartWatchCommandAsync(s)
                    ;
                    //probably i need to do a delay here to avoid response ddos to my server
                });
            });
        }

        public void InitializePushNotificationWatchTimer(int updatePeriod)
        {
            if (UpdatesHandler == null)
                throw new InvalidOperationException($"{nameof(ServiceFactory)} property must be initialized first.");
            if (UpdatesHandler == null)
                throw new InvalidOperationException($"{nameof(MessageHandler)} property must be initialized first.");

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
                    MessageHandler?.HandleStartWatchCommand(service);
                });
            }, null, updatePeriod, updatePeriod);
        }

        private async Task Updates_UpdatesTracingStoppedEvent(object sender, BotRequestErrorEventArgs e)
        {
            await CoffeeJTools.Delay(5000);
            ((LongPollingUpdates)Updates).Start();
        }



        public static BotInitializer Instance { get; private set; }

        public BotSettings BotSettings { get; set; }

        public IUpdate Updates { get; private set; }

        public UpdatesHandler UpdatesHandler { get; private set; }

        public Authorizer Authorizer { get; set; }

        public ServiceFactory ServiceFactory { get; set; }

        public MessageHandler MessageHandler { get; set; }

        public CallbackQueryHandler CallbackQueryHandler { get; set; }

        public InlineQueryHandler InlineQueryHandler { get; set; }

        public ChosenInlineResultHandler ChosenInlineResultHandler { get; set; }

        public NotifyHandler NotifyHandler { get; set; }

        public Timer PushNotificationWatchTimer { get; private set; }

        private static readonly object Locker = new object();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private GmailDbContextWorker _gmailDbContextWorker;
    }
}