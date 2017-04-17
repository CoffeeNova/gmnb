using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using Newtonsoft.Json.Linq;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    /// <summary>
    /// Interface for Telegram's server updates.
    /// </summary>
    public interface IUpdates
    {
        /// <summary>
        /// A container which comprises telegram request presented as JSON objects.
        /// </summary>
        List<Update> UpdatesList { get;}

        /// <summary>
        /// First update ID in a <see cref="UpdatesList"/> container. 
        /// </summary>
        long FirstUpdateId { get; }

        /// <summary>
        /// Last update ID in a <see cref="UpdatesList"/> container. 
        /// </summary>
        long LastUpdateId { get; }
    }
}
