using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.Message
{
    internal delegate Task HandleMessageCommand(ISender sender);

    internal interface IMessageHandlerRules
    {
        HandleMessageCommand Handle(TextMessage message, MessageHandler handler);
    }
}