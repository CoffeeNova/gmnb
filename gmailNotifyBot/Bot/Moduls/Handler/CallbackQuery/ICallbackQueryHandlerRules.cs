using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.TelegramBotApiWrapper.Types;


namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.Handler.CallbackQuery
{
    using Query = TelegramBotApiWrapper.Types.General.CallbackQuery;
    internal delegate Task HandleCallbackQueryCommand(Query sender);

    internal interface ICallbackQueryHandlerRules
    {
        HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler);
    }
}