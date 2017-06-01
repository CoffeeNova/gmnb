using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates
{
    internal interface IMessageHandlerRules
    {
        HandleMessageCommand Handle(Message message, MessageHandler handler);
    }
}