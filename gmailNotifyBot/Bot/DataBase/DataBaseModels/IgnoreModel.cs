using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class IgnoreModel
    {

        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("UserSettingsModelId")]
        public UserSettingsModel UserSettingsModel { get; set; }

        [Key, Column(Order = 2)]
        public int UserSettingsModelId { get; set; }

        public string Address { get; set; }
    }
}