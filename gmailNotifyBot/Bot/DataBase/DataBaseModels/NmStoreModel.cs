using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class NmStoreModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MessageId { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public List<string> To { get; set; } = new List<string>();

        public List<string> Cc { get; set; } = new List<string>();

        public List<string> Bcc { get; set; } = new List<string>();

        public List<FileModel> File { get; set; } = new List<FileModel>();

    }
}