using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQueryUpdates
{
    using Query = TelegramBotApiWrapper.Types.General.CallbackQuery;
    internal delegate Task HandleCallbackQueryCommand(Query query);

    internal interface ICallbackQueryHandlerRule
    {
        HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler);
    }
}