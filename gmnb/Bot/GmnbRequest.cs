using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace CoffeeJelly.gmnb.Bot
{
    public class GmnbRequests : IGmnbRequests
    {
        public GmnbRequests(string token, int lastUpdateId = 0)
        {
            Token = token;
            LastUpdateId = lastUpdateId;
            if (RequestMonitorTimer.Enabled) return;

            RequestMonitorTimer.Elapsed += RequestMonitorTimer_Elapsed;
            RequestMonitorTimer.Start();
        }

        private GmnbRequests()
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
            if (e.Result.Length <= 23)
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
                var message = "Google mail notify bot response timeout. Can't download content.";
                LogMaker.Log(message, true);
                RequestMonitorTimer.Stop();
                RequestTracingStoppedEvent?.Invoke(this, new GmnbRequestErrorEventArgs(message));
                return;
            }
            
            DownloadBotRequests();
        }

        public delegate void GmnbRequestErrorEventHandler(object sender, GmnbRequestErrorEventArgs e);
        public delegate void GmnbRequestsEventHandler(IGmnbRequests requests);

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

        private static readonly Semaphore _downloadBotRequestsSemaphore = new Semaphore(1, 1);

        private static System.Timers.Timer RequestMonitorTimer { get; } = new System.Timers.Timer
        {
            Interval = MaxDelayServerResponce,
            AutoReset = true
        };


        private string Token { get; }

        public long FirstUpdateId { get; set; }
        public long LastUpdateId { get; set; }

        public List<JToken> Requests { get; set; }

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

    public class GmnbRequestErrorEventArgs : EventArgs
    {
        public GmnbRequestErrorEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Handled { get; set; }
        public string ErrorMessage { get;}
    }
}
