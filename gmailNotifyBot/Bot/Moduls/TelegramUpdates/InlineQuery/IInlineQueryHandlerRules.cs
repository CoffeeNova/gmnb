using System.Threading.Tasks;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.InlineQuery
{
    using Query = TelegramBotApiWrapper.Types.InlineQuery;

    internal delegate Task HandleInlineQueryCommand();
    internal interface IInlineQueryHandlerRules
    {
        HandleInlineQueryCommand Handle(Query query, InlineQueryHandler handler);
    }
}