using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CoffeeJelly.tempadll.Extensions;

namespace CoffeeJelly.gmailNotifyBot.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index(string code, string state, string error)
        {
            var listParams = new List<string>{ code, state, error };
            await WriteParamsToTestFileAsync(listParams);

            return null;
        }

        private async Task WriteParamsToTestFileAsync(List<string> parametrs)
        {
            //var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
            var path = HttpRuntime.AppDomainAppPath;
            var fileName = "testAuth.txt";
            using (var fs = new FileStream(path.PathFormatter() + fileName, FileMode.Append, FileAccess.Write))
            {
                foreach (var line in parametrs)
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(line + "\r\n");
                    await fs.WriteAsync(info, 0, info.Length);
                }
            }
        }
    }
}