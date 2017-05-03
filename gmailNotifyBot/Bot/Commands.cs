using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public static class Commands
    {
        public const string SETTINGS_COMMAND = @"/settings";
        public const string CONNECT_COMMAND = @"/connect";
        public const string TESTNAME_COMMAND = @"/testname";
        public const string TESTMESSAGE_COMMAND = @"/testmessage";
        public const string NEW_COMMAND = @"/new";
        public const string INBOX_COMMAND = @"/inbox";
        public const string INBOX_INLINE_QUERY_COMMAND = "Inbox:";
    }
}