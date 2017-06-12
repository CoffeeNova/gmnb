using System;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQueryUpdates
{
    internal class SendAuthorizeLinkRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQAuthorize(sender);

            if (data.Command.Equals(TextCommand.AUTHORIZE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ExpandRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQExpand(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.EXPAND_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class HideRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQHide(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.HIDE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ExpandActionsRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQExpandActions(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.EXPAND_ACTIONS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class HideActionsRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQHideActions(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.HIDE_ACTIONS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToReadRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToRead(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.TO_READ_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToUnReadRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToUnRead(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.TO_UNREAD_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToSpamRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToSpam(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.TO_SPAM_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToInboxRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToInbox(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.TO_INBOX_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToTrashRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToTrash(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.TO_TRASH_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ArchiveRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQArchive(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.ARCHIVE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class UnignoreRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQUnignore(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.UNIGNORE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class IgnoreRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQIgnore(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.IGNORE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class NextPageRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQNextPage(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.NEXTPAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class PrevPageRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQPrevPage(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.PREVPAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ShowAttachmentsRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQShowAttachments(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.SHOW_ATTACHMENTS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class HideAttachmentsRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQHideAttachments(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.HIDE_ATTACHMENTS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }


    internal class GetAttachmentRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQGetAttachment(sender, data as GetCallbackData);
            if (data.Command.StartsWith(CallbackCommand.GET_ATTACHMENT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class AddTextMessageRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQAddTextMessage(sender, data as SendCallbackData);
            if (data.Command.StartsWith(CallbackCommand.ADD_TEXT_MESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class AddSubjectRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQAddSubject(sender, data as SendCallbackData);
            if (data.Command.StartsWith(CallbackCommand.ADD_SUBJECT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class SaveAsDraftRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQSaveAsDraft(sender, data as SendCallbackData);
            if (data.Command.StartsWith(CallbackCommand.SAVE_AS_DRAFT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class NotSaveAsDraftRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQNotSaveAsDraft(sender, data as SendCallbackData);
            if (data.Command.StartsWith(CallbackCommand.NOT_SAVE_AS_DRAFT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class CotinueWithOldRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQContinueWithOld(sender, data as SendCallbackData);
            if (data.Command.StartsWith(CallbackCommand.CONTINUE_COMPOSE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ContinueFromDraftRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQContinueFromDraft(sender, data as SendCallbackData);
            if (data.Command.StartsWith(CallbackCommand.CONTINUE_FROM_DRAFT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class SendMessageRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQSendNewMessage(sender, data as SendCallbackData);
            if (data.Command.StartsWith(CallbackCommand.SEND_NEW_MESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class RemoveItemNewMessageRule : ICallbackQueryHandlerRules
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQRemoveItemNewMessage(sender, data as SendCallbackData);
            if (data.Command.StartsWith(CallbackCommand.REMOVE_ITEM_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }
}