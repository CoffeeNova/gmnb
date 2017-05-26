using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.Handler.CallbackQuery;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.Handler.InlineQuery
{
    internal class ShowInboxMessagesRule : IInlineQueryHandlerRules
    {
        public HandleInlineQueryCommand Handle(InlineQueryHandler handler)
        {
            HandleInlineQueryCommand del = async sender => await handler.HandleShowMessagesInlineQueryCommand(sender);

            if (data.Command.Equals(Commands.CONNECT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ShowAllMessagesRule : IInlineQueryHandlerRules
    {
        public HandleInlineQueryCommand Handle(InlineQueryHandler handler)
        {
            HandleInlineQueryCommand del = async sender => await handler.HandleShowMessagesInlineQueryCommand(sender);

            if (data.Command.Equals(Commands.CONNECT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }
}