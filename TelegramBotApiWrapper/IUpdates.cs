using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.TelegramBotApiWrapper
{
    /// <summary>
    /// Interface for Telegram's server updates.
    /// </summary>
    public interface IUpdate
    {
        /// <summary>
        /// A container which comprises telegram request presented as JSON objects.
        /// </summary>
        Update Update { get;}

        /// <summary>
        /// Last update ID in a <see cref="Update"/> container. 
        /// </summary>
        long LastUpdateId { get; }

        /// <summary>
        /// Triggers when new updates has been traced on the telegram server.
        /// </summary>
        event BotUpdatesEventHandler UpdatesArrivedEvent;

        /// <summary>
        /// List the types of updates you want your bot to receive.
        /// </summary>
        /// <remarks>
        /// See details <see href="https://core.telegram.org/bots/api#getting-updates"> here</see>>.
        /// </remarks>
        List<UpdateType> AllowedUpdates { get; set; }
    }

    public delegate void BotUpdatesErrorEventHandler(object sender, BotRequestErrorEventArgs e);
    public delegate void BotUpdatesEventHandler(IUpdate updates);
}
