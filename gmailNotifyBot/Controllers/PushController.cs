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
using CoffeeJelly.TelegramBotApiWrapper;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Controllers
{
    public class PushController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            try
            {
                Request.InputStream.Seek(0, SeekOrigin.Begin);
                var json = new StreamReader(Request.InputStream).ReadToEnd();
                var message = JsonConvert.DeserializeObject<GoogleNotifyMessage>(json);
                var notifyHandler = BotInitializer.Instance?.NotifyHandler;
                TestModel.WritePushedMessageToTestFile(message);
                if (notifyHandler == null) return new HttpStatusCodeResult(HttpStatusCode.OK);

                notifyHandler.HandleGoogleNotifyMessage(message);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult TelegramPath()
        {
            try
            {
                Request.InputStream.Seek(0, SeekOrigin.Begin);
                var json = new StreamReader(Request.InputStream).ReadToEnd();
                var updates = BotInitializer.Instance?.Updates;
                if (updates == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                (updates as WebhookUpdates).HandleTelegramRequest(json);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                TestModel.WriteLogToFile(ex.Message + Environment.NewLine + ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
    }
}