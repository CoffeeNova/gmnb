using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represents one button of an inline keyboard. You must use exactly one of the optional fields.
    /// </summary>
    /// <remarks>
    /// After Bot API 3.0. release inline keyboards with <see cref="SwitchInlineQuery"/> and <see cref="SwitchInlineQueryCurrentChat"/> 
    /// can no longer be sent to channels.
    /// </remarks>
    public class InlineKeyboardButton
    {
        /// <summary>
        /// Label text on the button.
        /// </summary>
        [JsonProperty("text", Required = Required.Always)]
        public string Text { get; set; }

        /// <summary>
        /// Optional. HTTP url to be opened when button is pressed.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Optional. Data to be sent in a <see cref="CallbackQuery"/> to the bot when button is pressed, 1-64 bytes.
        /// </summary>
        [JsonProperty("callback_data")]
        public string CallbackData { get; set; }

        /// <summary>
        /// Optional. If set, pressing the button will prompt the user to select one of their chats, open that chat and insert the bot‘s username and the specified inline query in the input field. 
        /// Can be empty, in which case just the bot’s username will be inserted.
        /// </summary>
        [JsonProperty("switch_inline_query")]
        public string SwitchInlineQuery { get; set; }

        /// <summary>
        /// Optional. If set, pressing the button will insert the bot‘s username and the specified inline query in the current chat's input field. 
        /// Can be empty, in which case only the bot’s username will be inserted.
        /// </summary>
        [JsonProperty("switch_inline_query_current_chat")]
        public string SwitchInlineQueryCurrentChat { get; set; }

        /// <summary>
        /// Optional. Description of the game that will be launched when the user presses the button.
        /// </summary>
        /// <remarks>
        /// This type of button must always be the first button in the first row.
        /// </remarks>
        [JsonProperty("callback_game")]
        public CallbackGame CallbackGame { get; set; }

        /// <summary>
        /// Optional. Specify True, to send a Pay button.
        /// </summary>
        /// <remarks>
        /// This type of button must always be the first button in the first row.
        /// </remarks>
        public bool? Pay { get; set; }
    }
}