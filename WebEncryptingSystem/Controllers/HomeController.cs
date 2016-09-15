using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebEncryptingSystem.Models;

namespace WebEncryptingSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                byte[] avatar = new byte[file.ContentLength];
                file.InputStream.Read(avatar, 0, file.ContentLength);

                var fileModel = new FileModel()
                {
                    File = System.Text.Encoding.ASCII.GetString(avatar),
                    Name = Path.GetFileName(file.FileName)
                };

                var json = JsonConvert.SerializeObject(fileModel);
                file.SaveAs(Server.MapPath("~/Files/" + fileModel.Name));

                return Json(json, JsonRequestBehavior.AllowGet);
            }

            return HttpNotFound("File not found");
        }
    }
}