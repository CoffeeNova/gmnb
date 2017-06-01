using System;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using MessageHandler = CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates.MessageHandler;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ReplyMessage
{
    internal class ARule : IDocMessageRules
    {
        public HandleMessageCommand Handle(DocumentMessage message, MessageHandler handler)
        {
            HandleMessageCommand del = async sender => await handler.HandleAuthorizeCommand(sender);

            if (message.Text.StartsWith(Commands.AUTHORIZE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    
}