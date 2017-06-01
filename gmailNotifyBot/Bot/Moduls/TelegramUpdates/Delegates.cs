using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoffeeJelly.TelegramBotApiWrapper.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates
{
    internal delegate Task HandleMessageCommand(ISender sender);
}