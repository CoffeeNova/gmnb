﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Models
{
    public static class TestModel
    {
        public static void WritePushedMessageToTestFile(GoogleNotifyMessage message)
        {
            try
            {
                var path = HttpRuntime.AppDomainAppPath;
                var fileName = "testPush.txt";
                using (var fs = new FileStream(path.PathFormatter() + fileName, FileMode.Append, FileAccess.Write))
                {
                    byte[] info = null;
                    info =
                        new UTF8Encoding(true).GetBytes(
                            $"data: {message.Message.Data}\r\nmessageId: {message.Message.MessageId}\r\nsubscription: {message.Subscription}\r\n");
                    //info = new UTF8Encoding(true).GetBytes("test");
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                var path = HttpRuntime.AppDomainAppPath;
                var fileName = "error.txt";
                using (var fs = new FileStream(path.PathFormatter() + fileName, FileMode.Append, FileAccess.Write))
                {
                    byte[] info = null;
                    info =
                        new UTF8Encoding(true).GetBytes(ex.ToString());
                    fs.Write(info, 0, info.Length);
                }
            }
        }

        public static void WriteLogToFile(string logMessage)
        {
            var path = HttpRuntime.AppDomainAppPath;
            var fileName = "log.txt";
            using (var fs = new FileStream(path.PathFormatter() + fileName, FileMode.Append, FileAccess.Write))
            {
                byte[] info = null;
                info = new UTF8Encoding(true).GetBytes(logMessage);
                fs.Write(info, 0, info.Length);
            }
        }
    }
}