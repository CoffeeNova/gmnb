using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    /// <summary>
    /// A class which permanently observes updates from Telegram's bot server and
    /// notifies subscribers about new request as list of JSON objects.
    /// </summary>
    public class BotRequests : IRequests
    {
        /// <summary>
        /// Primiry constructor of a class, automatically triggers observation of new requests from Telegram server.
        /// </summary>
        /// <param name="token">Telegram bot token.</param>
        /// <param name="lastUpdateId">Last update ID used as offset in Telegram's api getUpdates function. Default value is 0.</param>
        public BotRequests(string token, int lastUpdateId = 0)
        {
            Token = token;
            LastUpdateId = lastUpdateId;
            if (RequestMonitorTimer.Enabled) return;

            RequestMonitorTimer.Elapsed += RequestMonitorTimer_Elapsed;
            RequestMonitorTimer.Start();
        }

        private BotRequests()
        {
            
        }

        private void DownloadBotRequests()
        {
            using (var webClient = new WebClient())
            {
                var uri = new Uri(TelegramBotUrl + Token + "/getupdates?offset=" + (LastUpdateId + 1));
                webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
                webClient.DownloadStringAsync(uri);
            }
        }

        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Debug.WriteLine("New string download completed.");
            _downloadBotRequestsSemaphore.Release();
            if (e.Error != null)
            {
                LogMaker.Log(e.Error);
                return;
            }
            if (e.Result.Length <= EmptyUpdateSymbolCount)
                return;
                
            var newRequests = JsonConvert.DeserializeObject<JObject>(e.Result);

            Requests = newRequests["result"].ToList();
            FirstUpdateId = (long)(Requests.First()["update_id"] as JValue).Value;
            LastUpdateId = (long)(Requests.Last()["update_id"] as JValue).Value;

            RequestsArrivedEvent?.Invoke(this);

        }

        private void RequestMonitorTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //если образовалась очередь, выдать ошибку задержки ответа сервера
            if (!_downloadBotRequestsSemaphore.WaitOne(0))
            {
                var message = "Telegram server response timeout. Can't download content.";
                LogMaker.Log(message, true);
                RequestMonitorTimer.Stop();
                RequestTracingStoppedEvent?.Invoke(this, new BotRequestErrorEventArgs(message));
                return;
            }
            
            DownloadBotRequests();
        }

        public delegate void GmnbRequestErrorEventHandler(object sender, BotRequestErrorEventArgs e);
        public delegate void GmnbRequestsEventHandler(IRequests requests);

        /// <summary>
        /// Triggers when a class stops tracing request from telegram server.
        /// </summary>
        public static event GmnbRequestErrorEventHandler RequestTracingStoppedEvent;

        /// <summary>
        /// Triggers when new request has been traced on the telegram server.
        /// </summary>
        public static event GmnbRequestsEventHandler RequestsArrivedEvent;

        private const string TelegramBotUrl = "https://api.telegram.org/bot";
        private const int MaxDelayServerResponce = 2000;
        private const int EmptyUpdateSymbolCount = 23;

        private static readonly Semaphore _downloadBotRequestsSemaphore = new Semaphore(1, 1);

        private static System.Timers.Timer RequestMonitorTimer { get; } = new System.Timers.Timer
        {
            Interval = MaxDelayServerResponce,
            AutoReset = true
        };


        private string Token { get; }

        /// <summary>
        /// First update ID in a <see cref="Requests"/> container. 
        /// </summary>
        public long FirstUpdateId { get; private set; }

        /// <summary>
        /// Last update ID in a <see cref="Requests"/> container. 
        /// </summary>
        public long LastUpdateId { get; private set; }

        /// <summary>
        /// List of requests, presented as JSON objects.
        /// </summary>
        public List<JToken> Requests { get; private set; }

        public static class LogMaker
        {
            public static void Log(string message, bool isError)
            {
                DateTime currentDate = DateTime.Now;
                Logger.Info(message);
                NewMessage?.Invoke(message, currentDate, isError);
            }

            public static void Log(Exception ex)
            {
                DateTime currentDate = DateTime.Now;
                Logger.Error(ex);
                NewMessage?.Invoke(ex.Message, currentDate, true);
            }

            public delegate void MessageDelegate(string message, DateTime time, bool isError);
            public static event MessageDelegate NewMessage;
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        }
        
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
