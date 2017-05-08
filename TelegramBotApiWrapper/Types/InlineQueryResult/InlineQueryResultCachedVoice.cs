using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to a voice message stored on the Telegram servers. By default, this voice message will be sent by the user.
    /// Alternatively, you can use <see cref="InlineQueryResultCachedVoice.InputMessageContent"/>  to send a message with the specified content instead of the the voice message.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InlineQueryResultCachedVoice : InlineQueryResultCaption, IResultTitle
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required =Required.Always)]
        public override string Type => InlineQueryResultType.Voice;

        /// <summary>
        /// A valid file identifier for the voice message.
        /// </summary>
        [JsonProperty("voice_file_id", Required = Required.Always)]
        public string VoiceFileId { get; set; }

        /// <summary>
        /// Title for the result.
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }
    }
}
