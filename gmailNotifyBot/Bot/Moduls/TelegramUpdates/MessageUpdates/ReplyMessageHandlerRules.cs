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
            if (reply.Text.StartsWith(Commands.MESSAGE_FORCE_REPLY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
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
            if (reply.Text.StartsWith(Commands.SUBJECT_FORCE_REPLY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
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
            if (reply.Text.StartsWith(Commands.MESSAGE_FORCE_REPLY_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;

            return null;
        }
    }
}