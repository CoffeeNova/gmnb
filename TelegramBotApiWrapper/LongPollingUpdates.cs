using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Exceptions;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Methods;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.TelegramBotApiWrapper
{
    /// <summary>
    /// A class which permanently observes updates from Telegram's bot server and
    /// notifies subscribers about new updates using long polling.
    /// </summary>
    public class LongPollingUpdates : IUpdate
    {
        /// <summary>
        /// Primiry constructor of a class.
        /// </summary>
        /// <param name="token">Telegram bot token.</param>
        /// <param name="lastUpdateId">Last update ID used as offset in Telegram's api getUpdates function. Default value is 0.</param>
        public LongPollingUpdates(string token, int lastUpdateId = 0)
        {
            Token = token;
            LastUpdateId = lastUpdateId;
            _requestMonitorTimer = new System.Timers.Timer
            {
                Interval = MaxDelayServerResponse,
                AutoReset = true
            };

            _telegramMethods = new TelegramMethods(token);
            _requestMonitorTimer.Elapsed += RequestMonitorTimer_Elapsed;
        }

        /// <summary>
        /// Triggers monitoring of new updates from Telegram server.
        /// </summary>
        public void Start()
        {
            if (AllowedUpdates?.Count == 0)
                throw new InvalidOperationException($"{nameof(AllowedUpdates)} must be specified before starting.");
            _requestMonitorTimer.Start();
        }

        /// <summary>
        /// Stops monitoring of new updates from Telegram server.
        /// </summary>
        /// <param name="triggerStoppedEvent"></param>
        /// <param name="message"></param>
        public void Stop(bool triggerStoppedEvent = false, string message = null)
        {
            _requestMonitorTimer.Stop();
            if (!triggerStoppedEvent) return;

            if (message == null)
                message = "Monitoring of new updates was stopped manually";
            UpdatesTracingStoppedEvent?.Invoke(this, new BotRequestErrorEventArgs(message));
        }

        private void DownloadUpdates()
        {
            try
            {
                var updates = _telegramMethods.GetUpdates(LastUpdateId + 1, null, null, AllowedUpdates).Result;
                if (updates.Count == 0)
                    return;
                updates.ForEach(update =>
                {
                    Update = update;
                    LastUpdateId = update.UpdateId;
                    UpdatesArrivedEvent?.Invoke(this);
                });
            }
            catch (TelegramMethodsException ex)
            {
                var message = "An error occurred while receiving the update";
                UpdatesTracingStoppedEvent?.Invoke(this, new BotRequestErrorEventArgs(message, ex));
            }
            finally
            {
                DownloadBotRequestsSemaphore.Release();
            }
        }

        private void RequestMonitorTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //если образовалась очередь, выдать ошибку задержки ответа сервера
            if (!DownloadBotRequestsSemaphore.WaitOne(0))
            {
                var message = "Telegram server response timeout. Can't download content.";
                _requestMonitorTimer.Stop();
                UpdatesTracingStoppedEvent?.Invoke(this, new BotRequestErrorEventArgs(message));
                return;
            }

            DownloadUpdates();
        }

        /// <summary>
        /// Triggers when a class stops tracing updates from telegram server.
        /// </summary>
        public event BotUpdatesErrorEventHandler UpdatesTracingStoppedEvent;

        /// <summary>
        /// Triggers when new updates has been traced on the telegram server.
        /// </summary>
        public event BotUpdatesEventHandler UpdatesArrivedEvent;


        private static readonly Semaphore DownloadBotRequestsSemaphore = new Semaphore(1, 1);

        private readonly System.Timers.Timer _requestMonitorTimer;

        private readonly TelegramMethods _telegramMethods;

        private string Token { get; }

        private int _maxDelayServerResponse = 2000;

        /// <summary>
        /// Expected time to receive a response from the server in ms.
        /// If the response does not arrive during this time, the requests stop sending to the server and the <see cref="UpdatesTracingStoppedEvent"/> event is triggered.
        /// The smaller the value, the more frequent the server requests will be sent to get new updates.
        /// </summary>
        /// <remarks>Default: 2000</remarks>
        public int MaxDelayServerResponse
        {
            get
            {
                return _maxDelayServerResponse;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(MaxDelayServerResponse),
                        "There must be at least a little more");
                _maxDelayServerResponse = value;
            }
        }

        /// <summary>
        /// Last update ID in a <see cref="Update"/> container. 
        /// </summary>
        public long LastUpdateId { get; private set; }

        /// <summary>
        /// List of updates.
        /// </summary>
        public Update Update { get; private set; }

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
        public BotRequestErrorEventArgs(string errorMessage, Exception ex = null)
        {
            ErrorMessage = errorMessage;
            Exception = ex;
        }

        public bool Handled { get; set; }
        public string ErrorMessage { get; }
        public Exception Exception { get; }
    }


}
