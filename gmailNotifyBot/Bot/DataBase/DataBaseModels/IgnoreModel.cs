using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class IgnoreModel
    {

        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("NmStoreModelId")]
        public virtual UserSettingsModel UserSettingsModel { get; set; }

        [Key, Column(Order = 2)]
        public virtual int NmStoreModelId { get; set; }

        public string Address { get; set; }
    }
}