using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a contact with a phone number. By default, this contact will be sent by the user. 
    /// Alternatively, you can use <see cref="InlineQueryResultContact.InputMessageContent"/>  to send a message with the specified content instead of the contact.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after 9 April, 2016. Older clients will ignore them.</remarks>
    public class InlineQueryResultContact : InlineQueryResultContent, IResultThumb
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required =Required.Always)]
        public override string Type => InlineQueryResultType.Contact;

        /// <summary>
        /// Contact's phone number.
        /// </summary>
        [JsonProperty("phone_number", Required = Required.Always)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Contact's first name.
        /// </summary>
        [JsonProperty("first_name", Required = Required.Always)]
        public string FirstName { get; set; }

        /// <summary>
        /// Optional. Contact's last name.
        /// </summary>
        [JsonProperty("last_name", Required = Required.Default)]
        public string LastName { get; set; }

        /// <summary>
        /// Optional. Url of the thumbnail for the result.
        /// </summary>
        [JsonProperty("thumb_url", Required = Required.Default)]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// Optional. Thumbnail width.
        /// </summary>
        [JsonProperty("thumb_width", Required = Required.Default)]
        public int? ThumbWidth { get; set; }

        /// <summary>
        /// Optional. Thumbnail height.
        /// </summary>
        [JsonProperty("thumb_height", Required = Required.Default)]
        public int? ThumbHeight { get; set; }
    }
}
