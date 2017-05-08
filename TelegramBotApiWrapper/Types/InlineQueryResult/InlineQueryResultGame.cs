using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult
{
    /// <summary>
    /// Represents a Game.
    /// </summary>
    /// <remarks>This will only work in Telegram versions released after October 1, 2016. Older clients will not display any inline results if a game result is among them.</remarks>
    public class InlineQueryResultGame : InlineQueryResult
    {
        /// <summary>
        /// Type of the result.
        /// </summary>
        [JsonProperty("type", Required = Required.Always)]
        public override string Type => InlineQueryResultType.Game;

        /// <summary>
        /// Short name of the game
        /// </summary>
        [JsonProperty("game_short_name", Required = Required.Always)]
        public string GameShortName { get; set; }
    }
}
