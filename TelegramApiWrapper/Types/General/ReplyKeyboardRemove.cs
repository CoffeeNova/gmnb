using CoffeeJelly.TelegramApiWrapper.Types.Message;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.General
{
    /// <summary>
    /// Upon receiving a message with this object, Telegram clients will remove the current custom keyboard and display the default letter-keyboard. 
    /// By default, custom keyboards are displayed until a new keyboard is sent by a bot. 
    /// An exception is made for one-time keyboards that are hidden immediately after the user presses a button (see <see cref="ReplyKeyboardMarkup"/>).
    /// </summary>
    public class ReplyKeyboardRemove : IMarkup
    {
        /// <summary>
        /// Requests clients to remove the custom keyboard (user will not be able to summon this keyboard; 
        /// if you want to hide the keyboard from sight but keep it accessible, use <see cref="ReplyKeyboardMarkup.OneTimeKeyboard"/>)
        /// </summary>
        [JsonProperty("remove_keyboard", Required = Required.Always)]
        public bool RemoveKeyboard { get; } = true;

        /// <summary>
        /// Optional. Use this parameter if you want to show the keyboard to specific users only. 
        /// Targets: 1) users that are @mentioned in the <see cref="TextMessage.Text"/>; 
        /// 2) if the bot's message is a reply (has ReplyToMessageId), sender of the original message.
        /// </summary>
        /// <example>
        /// A user votes in a poll, bot returns confirmation message in reply to the vote and removes the keyboard for that user, 
        /// while still showing the keyboard with poll options to users who haven't voted yet.
        /// </example>
        [JsonProperty("selective")]
        public bool? Selective { get; set; }
    }
}