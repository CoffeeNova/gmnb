using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to an mp3 audio file. By default, this audio file will be sent by the user. 
    /// Alternatively, you can use <see cref="InlineQueryResultAudio.InputMessageContent"/> to send a message with the specified content instead of the audio.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InlineQueryResultAudio : InlineQueryResultCaption, IResulAudio, IResultTitle
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required =Required.Always)]
        public override string Type => InlineQueryResultType.Audio;

        /// <summary>
        /// A valid URL for the audio file.
        /// </summary>
        [JsonProperty("audio_url", Required = Required.Always)]
        public string AudioUrl { get; set; }

        /// <summary>
        /// Title for the result.
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Optional. Performer.
        /// </summary>
        [JsonProperty("performer")]
        public string Performer { get; set; }

        /// <summary>
        /// Optional. Audio duration in seconds.
        /// </summary>
        [JsonProperty("audio_duration")]
        public int? AudioDuration { get; set; }
    }
}
