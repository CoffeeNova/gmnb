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

        public static Task DeleteAsync(this DirectoryInfo di)
        {
            return Task.Run(() => di.Delete());
        }

        public static Task DeleteAsync(this DirectoryInfo di, bool recursive)
        {
            return Task.Run(() => di.Delete(recursive));
        }
    }
}