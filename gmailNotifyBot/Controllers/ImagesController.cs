using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CoffeeJelly.gmailNotifyBot.Models;

namespace CoffeeJelly.gmailNotifyBot.Controllers
{
    public class ImagesController : Controller
    {
        // GET: Images
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = int.MaxValue, Location = OutputCacheLocation.Any)]
        public FileResult Silhouette48()
        {
            var filePath = Server.MapPath("~/Content/Images/silhouette48.jpg");
            string fileType = "image/jpeg";
            string fileName = "silhouette48.jpg";
            return File(filePath, fileType, fileName);
        }
    }
}