using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class FileModel : INmStoreModelRelation
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Telegram's file id.
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// Google attachment id.
        /// </summary>
        public string AttachId { get; set; }

        [ForeignKey("NmStoreModelId")]
        public NmStoreModel NmStoreModel { get; set; }

        [Key, Column(Order = 2)]
        public int NmStoreModelId { get; set; }

        public string OriginalName { get; set; }

    }
}