using System.Threading.Tasks;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ChosenInlineResult
{
    using QueryResult = TelegramBotApiWrapper.Types.InlineQueryResult;

    internal delegate Task HandleChosenInlineResultCommand();
    internal interface IChosenInlineResultHandlerRules
    {
        HandleChosenInlineResultCommand Handle(QueryResult.ChosenInlineResult result, ChosenInlineResultHandler handler);
    }
}