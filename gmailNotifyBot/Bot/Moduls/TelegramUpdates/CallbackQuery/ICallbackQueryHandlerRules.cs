using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQuery
{
    using Query = TelegramBotApiWrapper.Types.General.CallbackQuery;
    internal delegate Task HandleCallbackQueryCommand(Query query);

    internal interface ICallbackQueryHandlerRules
    {
        HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler);
    }
}