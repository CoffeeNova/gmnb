using System;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.Message
{
    internal class AuthorizeRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(TextMessage message, MessageHandler handler)
        {
            HandleMessageCommand del = async sender => await handler.HandleAuthorizeCommand(sender);

            if (message.Text.StartsWith(Commands.AUTHORIZE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class TestMessageRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(TextMessage message, MessageHandler handler)
        {
            HandleMessageCommand del = async sender => await handler.HandleTestMessageCommand(sender);

            if (message.Text.StartsWith(Commands.TESTMESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class TestNameRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(TextMessage message, MessageHandler handler)
        {
            HandleMessageCommand del = async sender => await handler.HandleTestNameCommand(sender);
            if (message.Text.StartsWith(Commands.TESTNAME_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class TestThreadRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(TextMessage message, MessageHandler handler)
        {
            HandleMessageCommand del = async sender => await handler.HandleTestThreadCommand(sender);
            if (message.Text.StartsWith(Commands.TESTTHREAD_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class StartNotifyRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(TextMessage message, MessageHandler handler)
        {
            HandleMessageCommand del = async sender => await handler.HandleStartNotifyCommand(sender);
            if (message.Text.StartsWith(Commands.START_NOTIFY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class StopNotifyRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(TextMessage message, MessageHandler handler)
        {
            HandleMessageCommand del = async sender => await handler.HandleStopNotifyCommand(sender);
            if (message.Text.StartsWith(Commands.STOP_NOTIFY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class StartWatchRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(TextMessage message, MessageHandler handler)
        {
            HandleMessageCommand del = async sender => await handler.HandleStartWatchCommandAsync(sender);
            if (message.Text.StartsWith(Commands.START_WATCH_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class StopWatchRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(TextMessage message, MessageHandler handler)
        {
            HandleMessageCommand del = async sender => await handler.HandleStopWatchCommandAsync(sender);
            if (message.Text.StartsWith(Commands.STOP_WATCH_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }
    
    internal class NewMessageRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(TextMessage message, MessageHandler handler)
        {
            HandleMessageCommand del = async sender => await handler.HandleNewMessageCommand(sender);
            if (message.Text.StartsWith(Commands.NEW_MESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class GetInboxRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(TextMessage message, MessageHandler messageHandler)
        {
            HandleMessageCommand del = async sender => await messageHandler.HandleGetInboxMessagesCommand(sender);
            if (message.Text.StartsWith(Commands.INBOX_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class GetAllRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(TextMessage message, MessageHandler messageHandler)
        {
            HandleMessageCommand del = async sender => await messageHandler.HandleGetAllMessagesCommand(sender);
            if (message.Text.StartsWith(Commands.ALL_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }
}