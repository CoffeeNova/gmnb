using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AuthRedirect.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public ActionResult Index(string state, string code)
        {

            var uri = new Uri($"https://telegram.me/GmailNotifyBot?start=state{state}-code={code}");
            var request = WebRequest.Create(uri);

            request.GetResponse();
            return null;
        }

        private void SendMessage(string message, string chatID)
        {
            using (WebClient webClient = new WebClient())
            {
                NameValueCollection pars = new NameValueCollection();
                pars.Add("chat_id", chatID);
                pars.Add("text", message);
                webClient.UploadValues(Link + TelegramToken + "/sendMessage", pars);

            }
        }

        private const string Link = "https://api.telegram.org/bot";
        private const string TelegramToken = "252886092:AAHxtq8ZINX6WJXcT-MuQFoarH9-8Ppntl8";
    }
}