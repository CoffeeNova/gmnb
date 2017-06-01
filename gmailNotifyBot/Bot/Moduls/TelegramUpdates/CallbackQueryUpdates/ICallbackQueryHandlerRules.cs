using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQueryUpdates
{
    using Query = TelegramBotApiWrapper.Types.General.CallbackQuery;
    internal delegate Task HandleCallbackQueryCommand(Query query);

    internal interface ICallbackQueryHandlerRules
    {
        HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryUpdates.CallbackQueryHandler handler);
    }
}