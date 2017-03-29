using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CoffeeJelly.Bot.Bot
{
    public interface IBotRequests
    {
        List<JToken> Requests { get; set; }
        long FirstUpdateId { get; set; }
        long LastUpdateId { get; set; }
    }
}
