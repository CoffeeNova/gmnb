namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public static class TextCommand
    {
        public const string SETTINGS_COMMAND = "/settings";
        public const string AUTHORIZE_COMMAND = "/connect";
        public const string TEST_NAME_COMMAND = "/testname";
        public const string TEST_MESSAGE_COMMAND = "/testmessage";
        public const string TEST_THREAD_COMMAND = "/testthread";
        public const string TEST_DRAFT_COMMAND = "/testdraft";
        public const string NEW_MESSAGE_COMMAND = "/new";
        public const string INBOX_COMMAND = "/inbox";
        public const string ALL_COMMAND = "/all";
        public const string DRAFT_COMMAND = "/draft";
        public const string EDIT_COMMAND = "/edit";
        public const string START_NOTIFY_COMMAND = "/notifystart";
        public const string STOP_NOTIFY_COMMAND = "/notifystop";
        public const string START_WATCH_COMMAND = "/watchstart";
        public const string STOP_WATCH_COMMAND = "/watchstop";
    }

    public static class InlineQueryCommand
    {
        public const string INBOX_COMMAND = "inbox";
        public const string ALL_COMMAND = "all";
        public const string DRAFT_COMMAND = "draft";
        public const string EDIT_COMMAND = "edit";
        public const string TO_RECIPIENTS_COMMAND = "to";
        public const string CC_RECIPIENTS_COMMAND = "cc";
        public const string BCC_RECIPIENTS_COMMAND = "bcc";
    }

    public static class ForceReplyCommand
    {
        public const string MESSAGE_COMMAND = "Message:";
        public const string SUBJECT_COMMAND = "Subject:";
    }

    public static class CallbackCommand
    {
        #region received message
        public const string EXPAND_COMMAND = "100";
        public const string HIDE_COMMAND = "101";
        public const string EXPAND_ACTIONS_COMMAND = "102";
        public const string HIDE_ACTIONS_COMMAND = "103";
        public const string NEXTPAGE_COMMAND = "104";
        public const string PREVPAGE_COMMAND = "105";
        public const string TO_UNREAD_COMMAND = "106";
        public const string TO_READ_COMMAND = "107";
        public const string TO_SPAM_COMMAND = "108";
        public const string REMOVE_SPAM_COMMAND = "109";
        public const string TO_TRASH_COMMAND = "110";
        public const string TO_INBOX_COMMAND = "111";
        public const string ARCHIVE_COMMAND = "112";
        public const string IGNORE_COMMAND = "113";
        public const string UNIGNORE_COMMAND = "114";
        #endregion

        #region send message
        public const string ADD_SUBJECT_COMMAND = "200";
        public const string ADD_TEXT_MESSAGE_COMMAND = "201";
        public const string SHOW_ATTACHMENTS_COMMAND = "202";
        public const string HIDE_ATTACHMENTS_COMMAND = "203";
        public const string GET_ATTACHMENT_COMMAND = "204";
        public const string SAVE_AS_DRAFT_COMMAND = "205";
        public const string CONTINUE_COMPOSE_COMMAND = "206";
        public const string NOT_SAVE_AS_DRAFT_COMMAND = "207";
        public const string SEND_NEW_MESSAGE_COMMAND = "208";
        public const string CONTINUE_FROM_DRAFT_COMMAND = "209";
        public const string OPEN_WEB_COMMAND = "210";
        public const string REMOVE_ITEM_COMMAND = "211";

        #endregion

        #region Settings commands

        #region MainMenu commands

        public const string LABELS_COMMAND = "300";
        public const string PERMISSIONS_COMMAND = "301";
        public const string IGNORELIST_COMMAND = "302";
        public const string ABOUT_COMMAND = "309";

        #endregion

        #region Labels commands

        public const string DISPLAY_LABELS_COMMAND = "310";
        public const string CREATE_NEW_LABEL_COMMAND = "311";
        public const string REMOVE_LABEL_COMMAND = "312";
        public const string EDIT_LABEL_COMMAND = "313";
        public const string WHITELIST_COMMAND = "314";
        public const string BLACKLIST_COMMAND = "315";
        public const string LABELS_BACK_COMMAND = "319";

        #endregion

        #region Labels List commands

        public const string LABELSLIST_BACK_COMMAND = "320";

        #endregion

        #region Permission commands
        public const string CHANGE_PERMISSIONS_COMMAND = "330";
        public const string REVOKE_REPMISSIONS_COMMAND = "331";
        public const string REVOKE_VIA_WEB_COMMAND = "332";
        public const string PERMISSIONS_BACK_COMMAND = "339";

        #endregion

        #region Ignore commands
        public const string SHOW_IGNORE_COMMAND = "340";
        public const string ADD_TO_IGNORE_COMMAND = "341";
        public const string REMOVE_FROM_IGNORE_COMMAND = "342";
        public const string IGNORE_BACK_COMMAND = "349";

        #endregion
        #endregion
    }
}