using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    /// <summary>
    /// A class which permanently observes updates from Telegram's bot server and
    /// notifies subscribers about new request as list of JSON objects.
    /// </summary>
    public class Requests : IRequests
    {
        /// <summary>
        /// Primiry constructor of a class, automatically triggers observation of new requests from Telegram server.
        /// </summary>
        /// <param name="token">Telegram bot token.</param>
        /// <param name="lastUpdateId">Last update ID used as offset in Telegram's api getUpdates function. Default value is 0.</param>
        public Requests(string token, int lastUpdateId = 0)
        {
            Token = token;
            LastUpdateId = lastUpdateId;
            if (RequestMonitorTimer.Enabled) return;

            _telegramMethods = new TelegramMethods(token);
            AllowedUpdates = new List<UpdateType> {UpdateType.AllUpdates};
            RequestMonitorTimer.Elapsed += RequestMonitorTimer_Elapsed;
            RequestMonitorTimer.Start();
        }

        private Requests()
        {
            
        }

        private void DownloadBotRequests()
        {
            var updates = _telegramMethods.GetUpdates(LastUpdateId + 1, null, null, AllowedUpdates);
            using (var webClient = new WebClient())
            {
                var uri = new Uri(TelegramBotUrl + Token + "/getupdates?offset=" + (LastUpdateId + 1));
                webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
                webClient.DownloadStringAsync(uri);
            }
        }

        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //Debug.WriteLine("New string download completed.");
            DownloadBotRequestsSemaphore.Release();
            if (e.Error != null)
            {
                LogMaker.Log(Logger, e.Error);
                return;
            }
            if (e.Result.Length <= EmptyUpdateSymbolCount)
                return;
                
            var newRequests = JsonConvert.DeserializeObject<JObject>(e.Result);

            RequestList = newRequests["result"].ToList();
            FirstUpdateId = (long)(RequestList.First()["update_id"] as JValue).Value;
            LastUpdateId = (long)(RequestList.Last()["update_id"] as JValue).Value;

            RequestsArrivedEvent?.Invoke(this);

        }

        private void RequestMonitorTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //если образовалась очередь, выдать ошибку задержки ответа сервера
            if (!DownloadBotRequestsSemaphore.WaitOne(0))
            {
                var message = "Telegram server response timeout. Can't download content.";
                LogMaker.Log(Logger, message, true);
                RequestMonitorTimer.Stop();
                RequestTracingStoppedEvent?.Invoke(this, new BotRequestErrorEventArgs(message));
                return;
            }
            
            DownloadBotRequests();
        }

        public delegate void BotRequestErrorEventHandler(object sender, BotRequestErrorEventArgs e);
        public delegate void BotRequestsEventHandler(IRequests requests);

        /// <summary>
        /// Triggers when a class stops tracing request from telegram server.
        /// </summary>
        public static event BotRequestErrorEventHandler RequestTracingStoppedEvent;

        /// <summary>
        /// Triggers when new request has been traced on the telegram server.
        /// </summary>
        public static event BotRequestsEventHandler RequestsArrivedEvent;

        private const string TelegramBotUrl = "https://api.telegram.org/bot";
        private const int MaxDelayServerResponce = 2000;
        private const int EmptyUpdateSymbolCount = 23;

        private static readonly Semaphore DownloadBotRequestsSemaphore = new Semaphore(1, 1);
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static System.Timers.Timer RequestMonitorTimer { get; } = new System.Timers.Timer
        {
            Interval = MaxDelayServerResponce,
            AutoReset = true
        };

        private TelegramMethods _telegramMethods;


        private string Token { get; }

        /// <summary>
        /// First update ID in a <see cref="RequestList"/> container. 
        /// </summary>
        public long FirstUpdateId { get; private set; }

        /// <summary>
        /// Last update ID in a <see cref="RequestList"/> container. 
        /// </summary>
        public long LastUpdateId { get; private set; }

        /// <summary>
        /// List of requests, presented as JSON objects.
        /// </summary>
        public List<JToken> RequestList { get; private set; }

        /// <summary>
        /// List the types of updates you want your bot to receive.
        /// </summary>
        /// <remarks>
        /// See details <see href="https://core.telegram.org/bots/api#getting-updates"> here</see>>.
        /// </remarks>
        public List<UpdateType> AllowedUpdates { get; set; }
    }

    public class BotRequestErrorEventArgs : EventArgs
    {
        public BotRequestErrorEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Handled { get; set; }
        public string ErrorMessage { get;}
    }
}
