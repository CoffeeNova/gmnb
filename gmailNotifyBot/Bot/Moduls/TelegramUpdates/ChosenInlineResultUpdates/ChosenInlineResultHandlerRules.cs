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

            if (result.Query.StartsWith(InlineQueryCommand.INBOX_COMMAND, StringComparison.CurrentCultureIgnoreCase))
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

            if (result.Query.StartsWith(InlineQueryCommand.ALL_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class GetDraftMessagesRule : IChosenInlineResultHandlerRules
    {
        public HandleChosenInlineResultCommand Handle(QueryResult.ChosenInlineResult result, ChosenInlineResultUpdates.ChosenInlineResultHandler handler)
        {
            HandleChosenInlineResultCommand del = async () =>
            {
                await handler.HandleGetMesssagesChosenInlineResult(result);
            };

            if (result.Query.StartsWith(InlineQueryCommand.DRAFT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
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

            if (result.Query.StartsWith(InlineQueryCommand.TO_RECIPIENTS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
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

            if (result.Query.StartsWith(InlineQueryCommand.CC_RECIPIENTS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
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

            if (result.Query.StartsWith(InlineQueryCommand.BCC_RECIPIENTS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class GetEditDraftRule : IChosenInlineResultHandlerRules
    {
        public HandleChosenInlineResultCommand Handle(QueryResult.ChosenInlineResult result, ChosenInlineResultUpdates.ChosenInlineResultHandler handler)
        {
            HandleChosenInlineResultCommand del = async () =>
            {
                await handler.HandleEditDraftChosenInlineResult(result);
            };

            if (result.Query.StartsWith(InlineQueryCommand.EDIT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }
}