using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CoffeeJelly.gmailNotifyBot.Bot;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.gmailNotifyBot.Models;
using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Controllers
{
    public class PushController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            Request.InputStream.Seek(0, SeekOrigin.Begin);
            var json = new StreamReader(Request.InputStream).ReadToEnd();
            var message = JsonConvert.DeserializeObject<GoogleNotifyMessage>(json);
            var notifyHandler = BotInitializer.Instance?.NotifyHandler;

            if (notifyHandler == null) return new HttpStatusCodeResult(HttpStatusCode.OK);

            notifyHandler.HandleGoogleNotifyMessage(message);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
            //return commandHandler.HandleGoogleNotifyMessage(message)
            //    ? new HttpStatusCodeResult(HttpStatusCode.OK)
            //    : new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}