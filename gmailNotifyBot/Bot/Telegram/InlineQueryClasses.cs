using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Extensions;
using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
{
    /// <summary>
    /// Represents an incoming inline query. When the user sends an empty query, your bot could return some default or trending results.
    /// </summary>
    public class InlineQuery : ISender
    {
        /// <summary>
        /// Unique identifier for this query.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Sender.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Sender location, only for bots that request user location.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("location")]
        public Location Location { get; set; }

        /// <summary>
        /// Text of the query (up to 512 characters).
        /// </summary>
        [JsonProperty("query")]
        public string Query { get; set; }

        /// <summary>
        /// Offset of the results to be returned, can be controlled by the bot.
        /// </summary>
        [JsonProperty("offset")]
        public string Offset { get; set; }
    }

    /// <summary>
    /// Represents one result of an inline query.
    /// </summary>
    public abstract class InlineQueryResult
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type")]
        public abstract string Type { get;}

        /// <summary>
        /// Unique identifier for this result, 1-64 Bytes.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    /// <summary>
    /// Represents a link to an article or web page.
    /// </summary>
    public class InlineQueryResultArticle : InlineQueryResult, IThumb
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type")]
        public override string Type => InlineQueryResultType.Article;

        /// <summary>
        /// Title of the result.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Content of the message to be sent.
        /// </summary>
        [JsonProperty("input_message_content")]
        public IInputMessageContent InputMessageContent { get; set; }

        /// <summary>
        /// <see cref="InlineKeyboardMarkup"/> attached to the message.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("reply_markup")]
        public InlineKeyboardMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// URL of the result.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Pass True, if you don't want the URL to be shown in the message.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("hide_url")]
        public bool? HideUrl { get; set; }

        /// <summary>
        /// Short description of the result.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Url of the thumbnail for the result.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("thumb_nail")]
        public string ThumbUrl { get; set; }

        /// <summary>
        /// Thumbnail width.
        /// </summary>
        [JsonProperty("thumb_width")]
        public int? ThumbWidth { get; set; }

        /// <summary>
        /// Thumbnail height.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("thumb_height")]
        public int? ThumbHeight { get; set; }
    }

    public static class InlineQueryResultType
    {
        public static string Article => "article";

        public static string Gif => "gif";

        public static string Mpeg4Gif => "mpeg4_gif";

        public static string Video => "video";

        public static string Audio => "audio";

        public static string Voice => "voice";

        public static string Document => "document";

        public static string Location => "location";

        public static string Venue => "venue";

        public static string Contact => "contact";

        public static string Game => "game";

        public static string Photo => "photo";

        public static string Sticker => "sticker";
    }

    public class ChosenInlineResult
    {
        /// <summary>
        /// The unique identifier for the result that was chosen.
        /// </summary>
        [JsonProperty("result_id")]
        public string ResultId { get; set; }

        /// <summary>
        /// The user that chose the result.
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }

        /// <summary>
        /// Sender location, only for bots that request user location.
        /// </summary>
        ///<remarks>Optional.</remarks>
        [JsonProperty("location")]
        public Location Location { get; set; }

        /// <summary>
        /// Identifier of the sent inline message.
        /// </summary>
        /// <remarks>
        /// Optional.
        /// Available only if there is an inline keyboard attached to the message. Will be also received in <see cref="CallbackQuery"/> and can be used to edit the message.
        /// </remarks>
        [JsonProperty("inline_message_id")]
        public string InlineMessageId { get; set; }

        /// <summary>
        /// The query that was used to obtain the result.
        /// </summary>
        [JsonProperty("query")]
        public string Query { get; set; }
    }

    /// <summary>
    /// Represents the content of a text message to be sent as the result of an inline query.
    /// </summary>
    public class InputTextMessageContent : IInputMessageContent
    {
        private string _messageText;
        /// <summary>
        /// Text of the message to be sent, 1-4096 characters.
        /// </summary>
        [JsonProperty("message_text")]
        public string MessageText
        {
            get { return _messageText; }
            set
            {
                if (value !=null && !value.Length.InRange(1, 4096))
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value} Length should be from 1 to 4096 characters.");
                _messageText = value;
            }
        }

        /// <summary>
        /// Send Markdown or HTML, if you want Telegram apps to show bold, italic, fixed-width text or inline URLs in your bot's message.
        /// </summary>
        [JsonProperty("parse_mode")]
        public string ParseMode { get; set; }

        [JsonProperty("")]
        public bool? DisableWebPagePreview { get; set; }
    }

}