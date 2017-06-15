using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class TempDataModel
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        
        public string LabelId { get; set; }
    }
}