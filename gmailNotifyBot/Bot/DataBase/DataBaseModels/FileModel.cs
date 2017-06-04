using System.ComponentModel.DataAnnotations.Schema;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class FileModel : File
    {
        public int Id { get; set; }

        [ForeignKey("Id")]
        public virtual NmStoreModel NmStoreModel { get; set; }
        public string OriginalName { get; set; }
    }
}