using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Extensions
{
    public static class FileExtension
    {
        public static Task DeleteAsync(this FileInfo fi)
        {
            return Task.Run(() => fi.Delete());
        }
    }
}