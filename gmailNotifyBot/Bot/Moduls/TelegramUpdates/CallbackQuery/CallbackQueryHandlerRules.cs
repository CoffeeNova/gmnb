using System;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQuery
{
    internal class SendAuthorizeLinkRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQAuthorize(sender);

            if (data.Command.Equals(Commands.CONNECT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ExpandRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQExpand(sender, data);

            if (data.Command.Equals(Commands.EXPAND_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class HideRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQHide(sender, data);

            if (data.Command.Equals(Commands.HIDE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ExpandActionsRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQExpandActions(sender, data);

            if (data.Command.Equals(Commands.EXPAND_ACTIONS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class HideActionsRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQHideActions(sender, data);

            if (data.Command.Equals(Commands.HIDE_ACTIONS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToReadRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToRead(sender, data);

            if (data.Command.Equals(Commands.TO_READ_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToUnReadRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToUnRead(sender, data);

            if (data.Command.Equals(Commands.TO_UNREAD_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToSpamRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToSpam(sender, data);

            if (data.Command.Equals(Commands.TO_SPAM_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToInboxRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToInbox(sender, data);

            if (data.Command.Equals(Commands.TO_INBOX_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToTrashRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToTrash(sender, data);

            if (data.Command.Equals(Commands.TO_TRASH_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ArchiveRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQArchive(sender, data);

            if (data.Command.Equals(Commands.ARCHIVE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class UnignoreRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQUnignore(sender, data);

            if (data.Command.Equals(Commands.UNIGNORE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class IgnoreRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQIgnore(sender, data);

            if (data.Command.Equals(Commands.IGNORE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class NextPageRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQNextPage(sender, data);

            if (data.Command.Equals(Commands.NEXTPAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class PrevPageRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQPrevPage(sender, data);

            if (data.Command.Equals(Commands.PREVPAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class AddSubjectRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQAddSubject(sender, data);

            if (data.Command.Equals(Commands.ADD_SUBJECT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ShowAttachmentsRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQShowAttachments(sender, data);

            if (data.Command.Equals(Commands.SHOW_ATTACHMENTS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class HideAttachmentsRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQHideAttachments(sender, data);

            if (data.Command.Equals(Commands.HIDE_ATTACHMENTS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }


    internal class GetAttachmentRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQGetAttachment(sender, data);

            if (data.Command.StartsWith(Commands.GET_ATTACHMENT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

}