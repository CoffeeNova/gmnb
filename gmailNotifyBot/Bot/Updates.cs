using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    /// <summary>
    /// A class which permanently observes updates from Telegram's bot server and
    /// notifies subscribers about new updates.
    /// </summary>
    public class Updates : IUpdates
    {
        /// <summary>
        /// Primiry constructor of a class, automatically triggers observation of new updates from Telegram server.
        /// </summary>
        /// <param name="token">Telegram bot token.</param>
        /// <param name="lastUpdateId">Last update ID used as offset in Telegram's api getUpdates function. Default value is 0.</param>
        public Updates(string token, int lastUpdateId = 0)
        {
            Token = token;
            LastUpdateId = lastUpdateId;
            if (RequestMonitorTimer.Enabled) return;

            _telegramMethods = new TelegramMethods(token);
            AllowedUpdates = new List<UpdateType> {UpdateType.AllUpdates};
            RequestMonitorTimer.Elapsed += RequestMonitorTimer_Elapsed;
            RequestMonitorTimer.Start();
        }

        private Updates()
        {
            
        }

        private void DownloadUpdates()
        {
            try
            {
                var updates = _telegramMethods.GetUpdates(LastUpdateId + 1, null, null, AllowedUpdates);
                DownloadBotRequestsSemaphore.Release();
                if (updates.Count == 0)
                    return;
                UpdatesList = updates;
                FirstUpdateId = updates.First().UpdateId;
                LastUpdateId = updates.Last().UpdateId;
                UpdatesArrivedEvent?.Invoke(this);
            }
            catch (TelegramMethodsException ex)
            {
                LogMaker.Log(Logger, ex);
            }
        }

        private void RequestMonitorTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //если образовалась очередь, выдать ошибку задержки ответа сервера
            if (!DownloadBotRequestsSemaphore.WaitOne(0))
            {
                var message = "Telegram server response timeout. Can't download content.";
                LogMaker.Log(Logger, message, true);
                RequestMonitorTimer.Stop();
                UpdatesTracingStoppedEvent?.Invoke(this, new BotRequestErrorEventArgs(message));
                return;
            }
            
            DownloadUpdates();
        }

        public delegate void BotUpdatesErrorEventHandler(object sender, BotRequestErrorEventArgs e);
        public delegate void BotUpdatesEventHandler(IUpdates updates);

        /// <summary>
        /// Triggers when a class stops tracing updates from telegram server.
        /// </summary>
        public static event BotUpdatesErrorEventHandler UpdatesTracingStoppedEvent;

        /// <summary>
        /// Triggers when new updates has been traced on the telegram server.
        /// </summary>
        public static event BotUpdatesEventHandler UpdatesArrivedEvent;

        private const int MaxDelayServerResponce = 2000;

        private static readonly Semaphore DownloadBotRequestsSemaphore = new Semaphore(1, 1);
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static System.Timers.Timer RequestMonitorTimer { get; } = new System.Timers.Timer
        {
            Interval = MaxDelayServerResponce,
            AutoReset = true
        };

        private readonly TelegramMethods _telegramMethods;


        private string Token { get; }

        /// <summary>
        /// First update ID in a <see cref="UpdatesList"/> container. 
        /// </summary>
        public long FirstUpdateId { get; private set; }

        /// <summary>
        /// Last update ID in a <see cref="UpdatesList"/> container. 
        /// </summary>
        public long LastUpdateId { get; private set; }

        /// <summary>
        /// List of updates.
        /// </summary>
        public List<Update> UpdatesList { get; private set; }

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
