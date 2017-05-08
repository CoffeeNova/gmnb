using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to a voice recording in an .ogg container encoded with OPUS. 
    /// By default, this voice recording will be sent by the user. 
    /// Alternatively, you can use <see cref="InlineQueryResultVoice.InputMessageContent"/>  to send a message with the specified content instead of the the voice message.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InlineQueryResultVoice : InlineQueryResultCaption, IResulVoice, IResultTitle
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required =Required.Always)]
        public override string Type => InlineQueryResultType.Voice;

        /// <summary>
        /// A valid URL for the voice recording.
        /// </summary>
        [JsonProperty("voice_url", Required = Required.Always)]
        public string VoiceUrl { get; set; }

        /// <summary>
        /// Title for the result.
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Optional. Recording duration in seconds.
        /// </summary>
        [JsonProperty("voice_duration")]
        public int? VoiceDuration { get; set; }
    }
}
