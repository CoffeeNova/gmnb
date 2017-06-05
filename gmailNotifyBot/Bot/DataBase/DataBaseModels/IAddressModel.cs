using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public interface IAddressModel
    {
        int Id { get; set; }

        string Address { get; set; }
    }
}