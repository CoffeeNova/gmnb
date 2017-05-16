using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public static class Commands
    {
        public const string SETTINGS_COMMAND = "/settings";
        public const string CONNECT_COMMAND = "/connect";
        public const string TESTNAME_COMMAND = "/testname";
        public const string TESTMESSAGE_COMMAND = "/testmessage";
        public const string TESTTHREAD_COMMAND = "/testthread";
        public const string NEW_MESSAGE_COMMAND = "/new";
        public const string INBOX_COMMAND = "/inbox";
        public const string ALL_COMMAND = "/all";
        public const string INBOX_INLINE_QUERY_COMMAND = "inbox";
        public const string ALL_INLINE_QUERY_COMMAND = "all";
        public const string EXPAND_COMMAND = "/expand";
        public const string HIDE_COMMAND = "/hide";
        public const string EXPAND_ACTIONS_COMMAND = "/expandActions";
        public const string HIDE_ACTIONS_COMMAND = "/hideActions";
        public const string NEXTPAGE_COMMAND = "/nextpage";
        public const string PREVPAGE_COMMAND = "/prevpage";
        public const string TO_UNREAD_COMMAND = "/tounread";
        public const string TO_READ_COMMAND = "/toread";
        public const string TO_SPAM_COMMAND = "/tospam";
        public const string REMOVE_SPAM_COMMAND = "/removespam";
        public const string TO_TRASHCOMMAND = "/totrash";
        public const string RESTORE_COMMAND = "/restore";
        public const string TO_INBOX_COMMAND = "/toinbox";
        public const string ARCHIVE_COMMAND = "/archive";
        public const string IGNORE_COMMAND = "/ignore";
        public const string UNIGNORE_COMMAND = "/unignore";
        public const string START_NOTIFY_COMMAND = "/notifystart";
        public const string STOP_NOTIFY_COMMAND = "/notifystop";
        public const string START_WATCH_COMMAND = "/watchstart";
        public const string STOP_WATCH_COMMAND = "/watchstop";
    }
}