﻿using System;
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
            NewMessage?.Invoke(logger, ex.Message, currentDate, true);
        }

        public delegate void MessageDelegate(Logger logger, string message, DateTime time, bool isError);
        public static event MessageDelegate NewMessage;
    }
}