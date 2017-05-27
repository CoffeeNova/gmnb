using System;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.InlineQuery
{
    using Query = TelegramBotApiWrapper.Types.InlineQuery;

    internal class ShowInboxMessagesRule : IInlineQueryHandlerRules
    {
        public HandleInlineQueryCommand Handle(Query query, InlineQueryHandler handler)
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
        public HandleInlineQueryCommand Handle(Query query, InlineQueryHandler handler)
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

    internal class ShowContactsRule : IInlineQueryHandlerRules
    {
        public HandleInlineQueryCommand Handle(Query query, InlineQueryHandler handler)
        {
            HandleInlineQueryCommand del = async () =>
            {
                await handler.HandleShowContactsInlineQueryCommand(query, Label.Sent);
            };

            if (query.Query.StartsWith(Commands.RECIPIENTS_INLINE_QUERY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }
}