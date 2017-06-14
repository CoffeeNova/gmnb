using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;


namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class UserSettingsModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool MailNotification { get; set; } = true;

        public long HistoryId { get; set; }

        public long Expiration { get; set; }

        public string Access { get; set; } = UserAccess.FULL;

        public bool UseWhitelist { get; set; }

        public List<IgnoreModel> IgnoreList { get; set; } = new List<IgnoreModel>();

        public List<LabelModel> Blacklist { get; set; } = new List<LabelModel>();

        public List<LabelModel> Whitelist { get; set; } = new List<LabelModel>();
    }

}