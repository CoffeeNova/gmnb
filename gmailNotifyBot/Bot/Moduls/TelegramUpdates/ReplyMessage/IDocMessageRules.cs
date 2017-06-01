using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using MessageHandler = CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates.MessageHandler;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ReplyMessage
{
    internal interface IDocMessageRules
    {
        HandleMessageCommand Handle(DocumentMessage message, MessageHandler handler);
    }
}