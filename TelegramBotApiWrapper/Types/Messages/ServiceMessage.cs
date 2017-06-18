using CoffeeJelly.TelegramBotApiWrapper.Types.Payments;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.Messages
{
    /// <summary>
    /// A class which represents the Telegram service message.
    /// </summary>
    public class ServiceMessage : Message
    {
        /// <summary>
        /// Service message: the chat photo was deleted.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("delete_chat_photo")]
        public bool? DeleteChatPhoto { get; set; }

        /// <summary>
        /// Service message: the group has been created.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("group_chat_created")]
        public bool? GroupChatCreated { get; set; }

        /// <summary>
        /// Service message: the supergroup has been created.
        /// </summary>
        /// <remarks>Optional. 
        /// This field can‘t be received in a message coming through updates, because bot can’t be a member of a supergroup when it is created. 
        /// It can only be found in reply_to_message if someone replies to a very first message in a directly created supergroup.
        /// </remarks>
        [JsonProperty("supergroup_chat_created")]
        public bool? SupergroupChatCreated { get; set; }

        /// <summary>
        /// Service message: the channel has been created. 
        /// </summary>
        /// <remarks>Optional.
        /// his field can‘t be received in a message coming through updates, because bot can’t be a member of a channel when it is created. 
        /// It can only be found in reply_to_message if someone replies to a very first message in a channel.
        /// </remarks>
        [JsonProperty("channel_chat_created")]
        public bool? ChannelChatCreated { get; set; }

        /// <summary>
        /// Message is a service message about a successful payment, information about the payment.
        /// </summary>
        ///<remarks> Optional.</remarks>
        public SuccessfulPayment SuccessfulPayment { get; set; }
    }
}