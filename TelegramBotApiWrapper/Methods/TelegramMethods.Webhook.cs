using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Attributes;
using CoffeeJelly.TelegramBotApiWrapper.Exceptions;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CoffeeJelly.TelegramBotApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        /// <summary>
        /// Use this method to specify a url and receive incoming updates via an outgoing webhook. 
        /// Whenever there is an update for the bot, we will send an HTTPS POST request to the specified url, containing a JSON-serialized <see cref="Update"/>. 
        /// In case of an unsuccessful request, we will give up after a reasonable amount of attempts. Returns true.
        /// </summary>
        /// <param name="url">HTTPS url to send updates to. Use an empty string to remove webhook integration.</param>
        /// <param name="certificate">
        /// The full file path of the certificate.
        /// Upload your public key certificate so that the root certificate in use can be checked. 
        /// See <see href="https://core.telegram.org/bots/self-signed">this guide</see> for details.
        /// </param>
        /// <param name="maxConnections">Maximum allowed number of simultaneous HTTPS connections to the webhook for update delivery, 1-100. 
        /// Defaults to 40. Use lower values to limit the load on your bot‘s server, and higher values to increase your bot’s throughput.</param>
        /// <param name="allowedUpdates">
        /// List the types of updates you want your bot to receive. If not specified, the previous setting will be used.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If <paramref name="url"/> is a null reference.</exception>
        /// <exception cref="TelegramMethodsException"></exception>
        [TelegramMethod("setWebhook", "certificate")]
        public async Task<bool> SetWebhook(string url, string certificate = null, int maxConnections = 40, List<UpdateType> allowedUpdates = null)
        {
            url.NullInspect(nameof(url));
            if (maxConnections < 1 || maxConnections > 100)
                throw new ArgumentOutOfRangeException(nameof(maxConnections), "Must be in range of 1 to 100");
            if(certificate != null)
                if (!new FileInfo(certificate).Exists)
                    throw new ArgumentException("The file does not exists", certificate);

            using (var form = new MultipartFormDataContent())
            {
                form.Add(new StringContent(maxConnections.ToString(), Encoding.UTF8), "max_connections");
                if (allowedUpdates != null)
                {
                    var allowedUpdatesString = string.Join(",", allowedUpdates.Select(i => $"\"{UpdateAttribute.GetUpdateType(i)}\""));
                    form.Add(new StringContent($"[{allowedUpdatesString}]", Encoding.UTF8), "allowed_updates");
                }
                if (certificate != null)
                    AddFileDataContent(form, certificate);

                return await UploadFormMessageData<bool>(form);
            }
        }
    }
}
