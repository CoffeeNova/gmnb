using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to an mp3 audio file stored on the Telegram servers. By default, this audio file will be sent by the user.
    /// Alternatively, you can use <see cref="InlineQueryResultCachedAudio.InputMessageContent"/> to send a message with the specified content instead of the audio.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InlineQueryResultCachedAudio : InlineQueryResultCaption
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required =Required.Always)]
        public override string Type => InlineQueryResultType.Audio;

        /// <summary>
        /// A valid URL for the audio file.
        /// </summary>
        [JsonProperty("audio_file_id", Required = Required.Always)]
        public string AudioFileUrl { get; set; }
    }
}
