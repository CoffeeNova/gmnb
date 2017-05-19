using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CoffeeJelly.gmailNotifyBot.Bot;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls;

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
            Authorizer.HandleAuthResponse(code, state, error);
            return null;
        }

        [HttpGet]
        public ActionResult Token(string code, string state, string error)
        {
            return null;
        }

        private async Task WriteParamsToTestFileAsync(List<string> parametrs)
        {
            
            //var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
            var path = HttpRuntime.AppDomainAppPath;
            var fileName = "testAuth.txt";
            using (var fs = new FileStream(path.PathFormatter() + fileName, FileMode.Append, FileAccess.Write))
            {
                byte[] info = null;
                foreach (var line in parametrs)
                {
                    info = new UTF8Encoding(true).GetBytes(line + "\r\n");
                }
                if (info != null)
                {
                    await fs.WriteAsync(info, 0, info.Length);
                    
                }

            }
        }
    }
}