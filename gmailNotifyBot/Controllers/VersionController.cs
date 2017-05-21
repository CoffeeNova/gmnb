using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoffeeJelly.gmailNotifyBot.Controllers
{
    public class VersionController : Controller
    {
        // GET: Version
        public ActionResult Index()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            ViewBag.Message = fvi.FileVersion;
            return View();
        }
    }
}