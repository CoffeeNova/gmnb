using System;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates
{
    internal class AuthorizeRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage =  message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleAuthorizeCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.AUTHORIZE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class TestMessageRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleTestMessageCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.TEST_MESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class TestNameRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleTestNameCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.TEST_NAME_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class TestThreadRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleTestThreadCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.TEST_THREAD_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class TestDraftRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleTestDraftCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.TEST_DRAFT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class StartNotifyRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleStartNotifyCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.START_NOTIFY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class StopNotifyRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleStopNotifyCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.STOP_NOTIFY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class StartWatchRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleStartWatchCommandAsync(sender);
            if (textMessage.Text.StartsWith(TextCommand.START_WATCH_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class StopWatchRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleStopWatchCommandAsync(sender);
            if (textMessage.Text.StartsWith(TextCommand.STOP_WATCH_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }
    
    internal class NewMessageRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleNewMessageCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.NEW_MESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class GetInboxRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler messageHandler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await messageHandler.HandleGetInboxMessagesCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.INBOX_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class GetAllRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler messageHandler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await messageHandler.HandleGetAllMessagesCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.ALL_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class GetDraftRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler messageHandler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await messageHandler.HandleGetDraftMessagesCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.DRAFT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class ShowSettingsRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            if (textMessage.Text.StartsWith(TextCommand.SETTINGS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
            {
                HandleMessageCommand del = async sender => await handler.HandleSettingsCommand(textMessage);
                return del;
            }

            return null;
        }
    }
}