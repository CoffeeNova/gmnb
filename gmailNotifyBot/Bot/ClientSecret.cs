using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Converters;
using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class ClientSecret
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