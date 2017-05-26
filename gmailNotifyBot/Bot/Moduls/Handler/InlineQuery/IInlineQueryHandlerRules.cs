using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.Handler.InlineQuery
{
    using Query = TelegramBotApiWrapper.Types.InlineQuery;

    internal delegate Task HandleInlineQueryCommand(Query query);
    internal interface IInlineQueryHandlerRules
    {
        HandleInlineQueryCommand Handle(Query query);
    }
}