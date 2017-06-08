using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public interface INmStoreModel
    {
        int Id { get; set; }
        NmStoreModel NmStoreModel { get; set; }
        int NmStoreModelId { get; set; }
    }
}