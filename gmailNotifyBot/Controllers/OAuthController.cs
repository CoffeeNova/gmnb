using System;
using System.Net;
using System.Web.Mvc;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.gmailNotifyBot.Models;

namespace CoffeeJelly.gmailNotifyBot.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return null;
        }

        [HttpGet]
        public ActionResult Code(string code, string state, string error)
        {
            try
            {
                Authorizer.HandleAuthResponse(code, state, error);
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                TestModel.WriteLogToFile(ex.Message + Environment.NewLine + ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult Token(string code, string state, string error)
        {
            return null;
        }


    }
}