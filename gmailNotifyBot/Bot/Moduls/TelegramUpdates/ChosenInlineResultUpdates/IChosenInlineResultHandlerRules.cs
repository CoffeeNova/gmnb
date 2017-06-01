using System.Threading.Tasks;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ChosenInlineResultUpdates
{
    using QueryResult = TelegramBotApiWrapper.Types.InlineQueryResult;

    internal delegate Task HandleChosenInlineResultCommand();
    internal interface IChosenInlineResultHandlerRules
    {
        HandleChosenInlineResultCommand Handle(QueryResult.ChosenInlineResult result, ChosenInlineResultUpdates.ChosenInlineResultHandler handler);
    }
}