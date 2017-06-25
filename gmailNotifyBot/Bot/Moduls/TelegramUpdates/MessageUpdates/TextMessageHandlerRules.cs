using System;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates
{
    internal class StartRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleStartCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.START_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class AuthorizeRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service,  MessageHandler handler)
        {
            var textMessage =  message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleAuthorizeCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.AUTHORIZE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class TestMessageRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service,  MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleTestMessageCommand(sender, service);
            if (textMessage.Text.StartsWith(TextCommand.TEST_MESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class TestNameRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service,  MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleTestNameCommand(service);
            if (textMessage.Text.StartsWith(TextCommand.TEST_NAME_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class TestThreadRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service,  MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleTestThreadCommand(service);
            if (textMessage.Text.StartsWith(TextCommand.TEST_THREAD_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class TestDraftRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service,  MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleTestDraftCommand(sender, service);
            if (textMessage.Text.StartsWith(TextCommand.TEST_DRAFT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class StartNotifyRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service,  MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleStartNotifyCommand(service);
            if (textMessage.Text.StartsWith(TextCommand.START_NOTIFY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class StopNotifyRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service,  MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleStopNotifyCommand(service);
            if (textMessage.Text.StartsWith(TextCommand.STOP_NOTIFY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class StartWatchRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service,  MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleStartWatchCommandAsync(service);
            if (textMessage.Text.StartsWith(TextCommand.START_WATCH_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class StopWatchRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service,  MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleStopWatchCommandAsync(service);
            if (textMessage.Text.StartsWith(TextCommand.STOP_WATCH_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }
    
    internal class NewMessageRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service,  MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await handler.HandleNewMessageCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.NEW_MESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class HelpRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service,  MessageHandler messageHandler)
        {
            var textMessage = message as TextMessage;
            if (textMessage == null) return null;

            HandleMessageCommand del = async sender => await messageHandler.HandleHelpCommand(sender);
            if (textMessage.Text.StartsWith(TextCommand.HELP_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class ShowSettingsRule : IMessageHandlerRule
    {
        public HandleMessageCommand Handle(Message message, Service service,  MessageHandler handler)
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