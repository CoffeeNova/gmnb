using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQuery
{
    using Query = TelegramBotApiWrapper.Types.General.CallbackQuery;
    internal delegate Task HandleCallbackQueryCommand(Query sender);

    internal interface ICallbackQueryHandlerRules
    {
        HandleCallbackQueryCommand Handle(ICallbackCommand data, CallbackQueryHandler handler);
    }
}