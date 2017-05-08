using System.Collections.Generic;
using Newtonsoft.Json;
using CoffeeJelly.TelegramBotApiWrapper.Converters;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class Secrets
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("auth_uri")]
        public string AuthUri { get; set; }

        [JsonProperty("token_uri")]
        public string TokeUri { get; set; }

        [JsonProperty("auth_provider_x509_cert_url")]
        public string AuthProvider { get; set; }

        [JsonProperty("client_secret")]
        public string Secret { get; set; }

        [JsonConverter(typeof(ArrayToListConverter<string>))]
        [JsonProperty("redirect_uris")]
        public List<string> RedirectUris { get; set; }
    }
}