using System;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates
{
    internal class MessageForceReplyRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage?.Text == null)
                return null;
            var reply = message.ReplyToMessage as TextMessage;
            if (reply == null)
                return null;

            HandleMessageCommand del = async sender => await handler.HandleMessageForceReply(textMessage);
            if (reply.Text.StartsWith(ForceReplyCommand.MESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class SubjectForceReplyRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage?.Text == null)
                return null;
            var reply = message.ReplyToMessage as TextMessage;
            if (reply == null)
                return null;

            HandleMessageCommand del = async sender => await handler.HandleSubjectForceReply(textMessage);
            if (reply.Text.StartsWith(ForceReplyCommand.SUBJECT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    internal class FileForceReplyRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var docMessage = message as DocumentMessage;

            var reply = docMessage?.ReplyToMessage as TextMessage;
            if (reply == null)
                return null;

            HandleMessageCommand del = async sender => await handler.HandleFileForceReply(docMessage);
            if (reply.Text.StartsWith(ForceReplyCommand.MESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }

    #region Settings

    internal class CreateNewLabelForceReplyRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage?.Text == null)
                return null;
            var reply = message.ReplyToMessage as TextMessage;
            if (reply == null)
                return null;

            if (!reply.Text.StartsWith(ForceReplyCommand.NEW_LABEL_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleMessageCommand del = async sender => await handler.HandleCreateNewLabelForceReply(textMessage);
            return del;
        }
    }

    internal class EditLabelNameForceReplyRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage?.Text == null)
                return null;
            var reply = message.ReplyToMessage as TextMessage;
            if (reply == null)
                return null;

            if (!reply.Text.StartsWith(ForceReplyCommand.EDIT_LABEL_NAME_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleMessageCommand del = async sender => await handler.HandleEditLabelNameForceReply(textMessage);
            return del;
        }
    }

    internal class AddToIgnoreForceReplyRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage?.Text == null)
                return null;
            var reply = message.ReplyToMessage as TextMessage;
            if (reply == null)
                return null;

            if (!reply.Text.StartsWith(ForceReplyCommand.ADD_TO_IGNORE_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleMessageCommand del = async sender => await handler.HandleAddToIgnoreForceReply(textMessage);
            return del;
        }
    }

    internal class RemoveFromIgnoreForceReplyRule : IMessageHandlerRules
    {
        public HandleMessageCommand Handle(Message message, MessageHandler handler)
        {
            var textMessage = message as TextMessage;
            if (textMessage?.Text == null)
                return null;
            var reply = message.ReplyToMessage as TextMessage;
            if (reply == null)
                return null;

            if (!reply.Text.StartsWith(ForceReplyCommand.REMOVE_FROM_IGNORE_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleMessageCommand del = async sender => await handler.HandleRemoveFromIgnoreForceReply(textMessage);
            return del;
        }
    }

    #endregion
}