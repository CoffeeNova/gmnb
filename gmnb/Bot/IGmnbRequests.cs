using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CoffeeJelly.gmnb.Bot
{
    public interface IGmnbRequests
    {
        List<JToken> Requests { get; set; }
        long FirstUpdateId { get; set; }
        long LastUpdateId { get; set; }
    }
}
