﻿using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to a video animation (H.264/MPEG-4 AVC video without sound) stored on the Telegram servers. 
    /// By default, this animated MPEG-4 file will be sent by the user with an optional caption.
    /// Alternatively, you can use <see cref="InlineQueryResultCachedMpeg4Gif.InputMessageContent"/>  to send a message with the specified content instead of the animation.
    /// </summary>
    public class InlineQueryResultCachedMpeg4Gif : InlineQueryResultCaption, IResultTitle
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public override string Type => InlineQueryResultType.Mpeg4Gif;

        /// <summary>
        /// A valid file identifier for the MP4 file.
        /// </summary>
        [JsonProperty("mpeg4_file_id", Required = Required.Always)]
        public string Mpeg4FileId { get; set; }

        /// <summary>
        /// Optional. Title for the result.
        /// </summary>
        public string Title { get; set; }
    }
}
