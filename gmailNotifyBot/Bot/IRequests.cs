using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    /// <summary>
    /// Interface for Telegram's server updates.
    /// </summary>
    public interface IRequests
    {
        /// <summary>
        /// A container which comprises telegram request presented as JSON objects.
        /// </summary>
        List<JToken> RequestList { get;}

        /// <summary>
        /// First update ID in a <see cref="RequestList"/> container. 
        /// </summary>
        long FirstUpdateId { get; }

        /// <summary>
        /// Last update ID in a <see cref="RequestList"/> container. 
        /// </summary>
        long LastUpdateId { get; }
    }
}
