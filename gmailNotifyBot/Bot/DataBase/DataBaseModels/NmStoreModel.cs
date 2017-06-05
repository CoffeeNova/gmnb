using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class NmStoreModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MessageId { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public virtual ICollection<ToModel> To { get; set; } = new List<ToModel>();

        public virtual ICollection<CcModel> Cc { get; set; } = new List<CcModel>();

        public virtual ICollection<BccModel> Bcc { get; set; } = new List<BccModel>();

        public virtual ICollection<FileModel> File { get; set; } = new List<FileModel>();

    }
}