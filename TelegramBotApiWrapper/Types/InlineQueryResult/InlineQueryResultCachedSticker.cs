using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a link to a sticker stored on the Telegram servers. By default, this sticker will be sent by the user. 
    /// Alternatively, you can use <see cref="InlineQueryResultCachedSticker.InputMessageContent"/>  to send a message with the specified content instead of the sticker.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InlineQueryResultCachedSticker : InlineQueryResultContent
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public override string Type => InlineQueryResultType.Sticker;

        /// <summary>
        /// A valid file identifier of the sticker.
        /// </summary>
        [JsonProperty("sticker_file_id", Required = Required.Always)]
        public string StickerFileId { get; set; }

    }
}
