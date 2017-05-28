using System.Linq;
using System.Reflection;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class BotSettings
    {
        public bool AllSettingsAreSet()
        {
            var propertiesInfo = this.GetType().GetProperties(BindingFlags.Instance |  BindingFlags.Public);
            return propertiesInfo.All(p => p.GetValue(this) != null);
        }

        public string Token { get; set; }

        public Secrets ClientSecrets { get; set; }

        public string BotName { get; set; }

        public string Topic { get; set; }

        public string Subscription { get; set; }

        public string ImagesPath { get; set; }

        public string DomainName { get; set; }

        public string AttachmentsTempFolder { get; set; }

        public int MaxAttachmentSize { get; set; }
    }
}