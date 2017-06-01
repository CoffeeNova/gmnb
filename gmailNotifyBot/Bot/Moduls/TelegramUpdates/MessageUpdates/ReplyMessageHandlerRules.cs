using System;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ReplyMessage;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates
{
    internal class MessageForceReplyRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var replyTextMessage = message?.ReplyToMessage as TextMessage;
            if (replyTextMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleMessageForceReply(message);
            if (replyTextMessage.Text.StartsWith(Commands.MESSAGE_FORCE_REPLY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    
}