using System;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQueryUpdates
{
    internal class SendAuthorizeLinkRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQAuthorize(sender);

            if (data.Command.Equals(TextCommand.AUTHORIZE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ExpandRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQExpand(sender, service, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.EXPAND_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class HideRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQHide(sender, service, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.HIDE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ExpandActionsRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQExpandActions(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.EXPAND_ACTIONS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class HideActionsRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQHideActions(sender, service, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.HIDE_ACTIONS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToReadRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToRead(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.TO_READ_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToUnReadRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToUnRead(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.TO_UNREAD_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToSpamRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToSpam(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.TO_SPAM_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToInboxRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToInbox(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.TO_INBOX_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ToTrashRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQToTrash(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.TO_TRASH_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ArchiveRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQArchive(sender, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.ARCHIVE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class UnignoreRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQUnignore(sender, service, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.UNIGNORE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class IgnoreRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQIgnore(sender, service, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.IGNORE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class NextPageRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQNextPage(sender, service, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.NEXTPAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class PrevPageRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQPrevPage(sender, service, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.PREVPAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ShowAttachmentsRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQShowAttachments(sender, service, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.SHOW_ATTACHMENTS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class HideAttachmentsRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQHideAttachments(sender, service, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.HIDE_ATTACHMENTS_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }


    internal class GetAttachmentRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GetCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQGetAttachment(service, data as GetCallbackData);
            if (data.Command.Equals(CallbackCommand.GET_ATTACHMENT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class AddTextMessageRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQAddTextMessage(sender, data as SendCallbackData);
            if (data.Command.Equals(CallbackCommand.ADD_TEXT_MESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class AddSubjectRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQAddSubject(sender, data as SendCallbackData);
            if (data.Command.Equals(CallbackCommand.ADD_SUBJECT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class SaveAsDraftRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQSaveAsDraft(service, data as SendCallbackData);
            if (data.Command.Equals(CallbackCommand.SAVE_AS_DRAFT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class NotSaveAsDraftRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQNotSaveAsDraft(sender, data as SendCallbackData);
            if (data.Command.Equals(CallbackCommand.NOT_SAVE_AS_DRAFT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class CotinueWithOldRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQContinueWithOld(sender, data as SendCallbackData);
            if (data.Command.Equals(CallbackCommand.CONTINUE_COMPOSE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class ContinueFromDraftRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQContinueFromDraft(sender, data as SendCallbackData);
            if (data.Command.Equals(CallbackCommand.CONTINUE_FROM_DRAFT_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class SendMessageRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQSendNewMessage(sender, data as SendCallbackData, service);
            if (data.Command.Equals(CallbackCommand.SEND_NEW_MESSAGE_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    internal class RemoveItemNewMessageRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SendCallbackData))
                return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQRemoveItemNewMessage(sender, data as SendCallbackData);
            if (data.Command.Equals(CallbackCommand.REMOVE_ITEM_COMMAND, StringComparison.CurrentCultureIgnoreCase))
                return del;
            return null;
        }
    }

    #region Settings menu rules

    #region Main menu
    internal class OpenLabelsMenuRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.LABELS_MENU_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQLabelsMenu(sender);
            return del;
        }
    }

    internal class OpenPermissionsMenuRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.PERMISSIONS_MENU_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQPermissionsMenu(sender);
            return del;
        }
    }

    internal class OpenIgnoreListMenuRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.IGNORE_CONTROL_MENU_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQIgnoreMenu(sender);
            return del;
        }
    }

    internal class StartNotifyRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.NOTIFY_START_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQStartNotify(sender, service);
            return del;
        }
    }

    internal class StopNotifyRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.NOTIFY_STOP_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQStopNotify(sender, service);
            return del;
        }
    }

    internal class ShowAboutRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.ABOUT_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQAbout(sender, data as SettingsCallbackData);
            return del;
        }
    }
    #endregion

    #region Labels menu

    internal class OpenEditLabelsMenuRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.EDIT_LABELS_MENU_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQEditLabelsMenu(sender, service);
            return del;
        }
    }

    internal class CreateNewLabelRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.NEW_LABEL_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQCreateNewLabel(sender);
            return del;
        }
    }

    internal class OpenWhitelistMenuRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.WHITELIST_MENU_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQWhitelistMenu(sender, service);
            return del;
        }
    }

    internal class OpenBlacklistMenuRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.BLACKLIST_MENU_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQBlacklistMenu(sender, service);
            return del;
        }
    }
    #endregion

    #region Labels list

    internal class BackToLabelsMenuRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.LABELS_LIST_BACK_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQBackToLabelsMenu(sender);
            return del;
        }
    }

    internal class OpenLabelActionsMenuRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.LABEL_ACTIONS_MENU_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQLabelActionsMenu(sender, data as SettingsCallbackData);
            return del;
        }
    }

    internal class WhitelistLabelActionRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.WHITELIST_ACTION_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQWhitelistLabelAction(sender, data as SettingsCallbackData, service);
            return del;
        }
    }

    internal class BlacklistLabelActionRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.BLACKLIST_ACTION_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQBlacklistLabelAction(sender, data as SettingsCallbackData, service);
            return del;
        }
    }

    internal class UseBlackListRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.USE_BLACKLIST_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQUseBlacklist(sender, service);
            return del;
        }
    }

    internal class UseWhiteListRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.USE_WHITELIST_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQUseWhitelist(sender, service);
            return del;
        }
    }
    #endregion

    #region Label actions menu

    internal class EditLabelNameRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.EDIT_LABEL_NAME_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQEditLabelName(sender);
            return del;
        }
    }

    internal class RemoveLabelRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.REMOVE_LABEL_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQRemoveLabel(sender, data as SettingsCallbackData, service);
            return del;
        }
    }

    internal class BackToEditLabelsListMenuRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.LABEL_ACTIONS_BACK_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQBackToEditLabelsListMenu(sender, service);
            return del;
        }
    }

    #endregion

    #region Ignore menu

    internal class DisplayIgnoredEmailRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.DISPLAY_IGNORE_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQDisplayIgnoredEmails(sender, data as SettingsCallbackData);
            return del;
        }
    }

    #endregion

    #region Permissions menu

    internal class AddToIgnoreRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.ADD_TO_IGNORE_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQAddToIgnore(sender);
            return del;
        }
    }

    internal class RemoveFromIgnoreRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.REMOVE_FROM_IGNORE_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQRemoveFromIgnore(sender);
            return del;
        }
    }

    internal class SwapPermissionsRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.SWAP_PERMISSIONS_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQSwapPermissions(sender, service);
            return del;
        }
    }

    internal class RevokePermissionsRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.REVOKE_REPMISSIONS_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQRevokePermissions(sender, service);
            return del;
        }
    }

    internal class RevokePermissionsViaWebRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.REVOKE_VIA_WEB_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQRevokePermissionsViaWeb(sender);
            return del;
        }
    }
    #endregion

    internal class BackToMainMenuRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is SettingsCallbackData))
                return null;

            if (!data.Command.EqualsAny(StringComparison.CurrentCultureIgnoreCase,
                CallbackCommand.LABELS_BACK_COMMAND,
                CallbackCommand.PERMISSIONS_BACK_COMMAND,
                CallbackCommand.IGNORE_BACK_COMMAND
                )) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQBackToMainMenu(sender);
            return del;
        }
    }

    #endregion

#region general rules

    internal class ResumeNotifyRule : ICallbackQueryHandlerRule
    {
        public HandleCallbackQueryCommand Handle(CallbackData data, Service service, CallbackQueryHandler handler)
        {
            if (!(data is GeneralCallbackData))
                return null;

            if (!data.Command.Equals(CallbackCommand.RESUME_NOTIFICATION_COMMAND,
                StringComparison.CurrentCultureIgnoreCase)) return null;

            HandleCallbackQueryCommand del = async sender => await handler.HandleCallbackQStartNotify(sender, service);
            return del;
        }
    }

#endregion
}