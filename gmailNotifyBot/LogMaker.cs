using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot;
using NLog;

namespace CoffeeJelly.gmailNotifyBot
{
    public static class LogMaker
    {
        public static void Log(Logger logger, string message, bool isError)
        {
            DateTime currentDate = DateTime.Now;
            logger.Info(message);
            NewMessage?.Invoke(logger, message, currentDate, isError);
        }

        public static void Log(Logger logger, Exception ex)
        {
            DateTime currentDate = DateTime.Now;
            logger.Error(ex);
            NewMessage?.Invoke(logger, ex.Message, currentDate, true, ex.StackTrace);
        }

        public static void Log(Logger logger, Exception ex, string message)
        {
            DateTime currentDate = DateTime.Now;
            logger.Error(ex, message);
            
            NewMessage?.Invoke(logger, ex.Message, currentDate, true, ex.StackTrace);
        }

        public delegate void MessageDelegate(Logger logger, string message, DateTime time, bool isError, string stackTrace=null);
        public static event MessageDelegate NewMessage;
    }
}