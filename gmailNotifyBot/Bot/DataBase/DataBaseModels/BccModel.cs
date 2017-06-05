﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class BccModel : IAddressModel
    {
        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("NmStoreModelId")]
        public NmStoreModel NmStoreModel { get; set; }

        [Key, Column(Order = 2)]
        public int NmStoreModelId { get; set; }

        public string Address { get; set; }
    }


}