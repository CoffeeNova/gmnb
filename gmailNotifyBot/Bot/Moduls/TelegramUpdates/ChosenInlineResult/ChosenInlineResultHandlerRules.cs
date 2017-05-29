using System;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ChosenInlineResult
{
    using QueryResult = TelegramBotApiWrapper.Types.InlineQueryResult;

    internal class GetInboxMessagesRule : IChosenInlineResultHandlerRules
    {
        public HandleChosenInlineResultCommand Handle(QueryResult.ChosenInlineResult result, ChosenInlineResultHandler handler)
        {
            HandleChosenInlineResultCommand del = async () =>
            {
                await handler.HandleGetMesssagesChosenInlineResult(result);
            };

            if (result.Query.Equals(Commands.INBOX_INLINE_QUERY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class GetAllMessagesRule : IChosenInlineResultHandlerRules
    {
        public HandleChosenInlineResultCommand Handle(QueryResult.ChosenInlineResult result, ChosenInlineResultHandler handler)
        {
            HandleChosenInlineResultCommand del = async () =>
            {
                await handler.HandleGetMesssagesChosenInlineResult(result);
            };

            if (result.Query.Equals(Commands.ALL_INLINE_QUERY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ShowContactsRule : IChosenInlineResultHandlerRules
    {
        public HandleChosenInlineResultCommand Handle(QueryResult.ChosenInlineResult result, ChosenInlineResultHandler handler)
        {
            HandleChosenInlineResultCommand del = async () =>
            {
                await handler.HandleSetToChosenInlineResult(result);
            };

            if (result.Query.StartsWith(Commands.RECIPIENTS_INLINE_QUERY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }
}