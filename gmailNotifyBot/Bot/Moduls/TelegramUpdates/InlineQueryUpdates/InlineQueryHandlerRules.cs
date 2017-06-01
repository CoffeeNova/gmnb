using System;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.InlineQueryUpdates
{
    using Query = TelegramBotApiWrapper.Types.InlineQuery;

    internal class ShowInboxMessagesRule : IInlineQueryHandlerRules
    {
        public HandleInlineQueryCommand Handle(Query query, InlineQueryUpdates.InlineQueryHandler handler)
        {
            HandleInlineQueryCommand del = async () =>
            {
                var arguments = Methods.CutArguments(query);
                var skip = Methods.ArgumentAssigment(ref arguments);
                await handler.HandleShowMessagesInlineQueryCommand(query, Label.Inbox, skip, arguments);
            };

            if (query.Query.StartsWith(Commands.INBOX_INLINE_QUERY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ShowAllMessagesRule : IInlineQueryHandlerRules
    {
        public HandleInlineQueryCommand Handle(Query query, InlineQueryUpdates.InlineQueryHandler handler)
        {
            HandleInlineQueryCommand del = async () =>
            {
                var arguments = Methods.CutArguments(query);
                var skip = Methods.ArgumentAssigment(ref arguments);
                await handler.HandleShowMessagesInlineQueryCommand(query, null, skip, arguments);
            };

            if (query.Query.StartsWith(Commands.ALL_INLINE_QUERY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ShowToContactsRule : IInlineQueryHandlerRules
    {
        public HandleInlineQueryCommand Handle(Query query, InlineQueryUpdates.InlineQueryHandler handler)
        {
            HandleInlineQueryCommand del = async () =>
            {
                await handler.HandleShowContactsInlineQueryCommand(query, Label.Sent);
            };

            if (query.Query.StartsWith(Commands.TO_RECIPIENTS_INLINE_QUERY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ShowCcContactsRule : IInlineQueryHandlerRules
    {
        public HandleInlineQueryCommand Handle(Query query, InlineQueryUpdates.InlineQueryHandler handler)
        {
            HandleInlineQueryCommand del = async () =>
            {
                await handler.HandleShowContactsInlineQueryCommand(query, Label.Sent);
            };

            if (query.Query.StartsWith(Commands.CC_RECIPIENTS_INLINE_QUERY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ShowBccContactsRule : IInlineQueryHandlerRules
    {
        public HandleInlineQueryCommand Handle(Query query, InlineQueryUpdates.InlineQueryHandler handler)
        {
            HandleInlineQueryCommand del = async () =>
            {
                await handler.HandleShowContactsInlineQueryCommand(query, Label.Sent);
            };

            if (query.Query.StartsWith(Commands.BCC_RECIPIENTS_INLINE_QUERY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }
}