using System;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ChosenInlineResultUpdates
{
    using QueryResult = TelegramBotApiWrapper.Types.InlineQueryResult;

    internal class GetInboxMessagesRule : IChosenInlineResultHandlerRules
    {
        public HandleChosenInlineResultCommand Handle(QueryResult.ChosenInlineResult result, ChosenInlineResultUpdates.ChosenInlineResultHandler handler)
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
        public HandleChosenInlineResultCommand Handle(QueryResult.ChosenInlineResult result, ChosenInlineResultUpdates.ChosenInlineResultHandler handler)
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

    internal class GetToContactsRule : IChosenInlineResultHandlerRules
    {
        public HandleChosenInlineResultCommand Handle(QueryResult.ChosenInlineResult result, ChosenInlineResultUpdates.ChosenInlineResultHandler handler)
        {
            HandleChosenInlineResultCommand del = async () =>
            {
                await handler.HandleSetToChosenInlineResult(result);
            };

            if (result.Query.StartsWith(Commands.TO_RECIPIENTS_INLINE_QUERY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class GetCcContactsRule : IChosenInlineResultHandlerRules
    {
        public HandleChosenInlineResultCommand Handle(QueryResult.ChosenInlineResult result, ChosenInlineResultUpdates.ChosenInlineResultHandler handler)
        {
            HandleChosenInlineResultCommand del = async () =>
            {
                await handler.HandleSetCcChosenInlineResult(result);
            };

            if (result.Query.StartsWith(Commands.CC_RECIPIENTS_INLINE_QUERY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class GetBccContactsRule : IChosenInlineResultHandlerRules
    {
        public HandleChosenInlineResultCommand Handle(QueryResult.ChosenInlineResult result, ChosenInlineResultUpdates.ChosenInlineResultHandler handler)
        {
            HandleChosenInlineResultCommand del = async () =>
            {
                await handler.HandleSetBccChosenInlineResult(result);
            };

            if (result.Query.StartsWith(Commands.BCC_RECIPIENTS_INLINE_QUERY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }
}