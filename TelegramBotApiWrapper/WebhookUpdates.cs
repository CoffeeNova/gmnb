using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Methods;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper
{
    public class WebhookUpdates : IUpdate
    {
        /// <summary>
        /// Primiry constructor of a class.
        /// </summary>
        /// <param name="token">Telegram bot token.</param>
        public WebhookUpdates(string token)
        {
            _telegramMethods = new TelegramMethods(token);
        }

        /// <summary>
        /// Use this method to specify a url and receive incoming updates via an outgoing webhook.  
        /// </summary>
        public async void SetWebhook()
        {
            if (Url == null)
                throw new InvalidOperationException($"Must specify {nameof(Url)} property.");

            await _telegramMethods.SetWebhook(Url, CertificatePath, MaxConnections, AllowedUpdates)
                .ConfigureAwait(false);
        }

        public void HandleTelegramRequest(string update)
        {
            var obj =  JsonConvert.DeserializeObject<Update>(update);
            LastUpdateId = obj.UpdateId;
            Update = obj;
            UpdatesArrivedEvent?.Invoke(this);
        }

        private readonly TelegramMethods _telegramMethods;

        public Update Update{ get; private set; }

        public long LastUpdateId { get; private set; }


        /// <summary>
        /// Triggers when new updates has been traced on the telegram server.
        /// </summary>
        public event BotUpdatesEventHandler UpdatesArrivedEvent;

        /// <summary>
        /// List the types of updates you want your bot to receive. If not specified, the previous setting will be used.
        /// </summary>
        public List<UpdateType> AllowedUpdates { get; set; }

        /// <summary>
        /// Maximum allowed number of simultaneous HTTPS connections to the webhook for update delivery, 1-100.
        /// </summary>
        public int MaxConnections { get; set; } = 40;

        /// <summary>
        /// HTTPS url to send updates to.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The full file path of the certificate.
        /// </summary>
        public string CertificatePath { get; set; }
    }
}
