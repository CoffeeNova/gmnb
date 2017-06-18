using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates
{
    internal interface IMessageHandlerRule
    {
        HandleMessageCommand Handle(Message message, Service service, MessageHandler handler);
    }
}