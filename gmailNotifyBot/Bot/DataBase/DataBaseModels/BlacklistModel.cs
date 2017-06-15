using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class BlacklistModel : ILabelInfo, IUserSettingsModelRelation
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("UserSettingsModelId")]
        public UserSettingsModel UserSettingsModel { get; set; }

        [Key, Column(Order = 2)]
        public int UserSettingsModelId { get; set; }

        public string Name { get; set; }

        public string LabelId { get; set; }
    }
}