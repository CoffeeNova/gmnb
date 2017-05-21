using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Bot.Types
{
    public class GoogleNotifyMessage
    {
        [JsonProperty("message")]
        public MessageBody Message { get; set; }

        [JsonProperty("subscription")]
        public string Subscription { get; set; }
    }

    public class MessageBody
    {
        [JsonProperty("attributes")]
        public AttributesBody Attributes { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("publish_time")]
        public string PublishTime { get; set; }
    }

    public class AttributesBody
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class EncodedMessageData
    {
        [JsonProperty("emailAddress")]
        public string Email { get; set; }

        [JsonProperty("historyId")]
        public string HistoryId { get; set; }
    }
}