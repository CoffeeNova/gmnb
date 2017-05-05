using CoffeeJelly.TelegramApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram game message.
    /// </summary>
    public class GameMessage : Message, IGameMessage
    {
        /// <summary>
        /// Information about the game.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("game")]
        public Game Game { get; set; }
    }
}