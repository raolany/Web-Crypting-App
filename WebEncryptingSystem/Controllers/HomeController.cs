using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public ActionResult Spectrum(string filename)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Files/" + filename);
                string txt = System.IO.File.ReadAllText(path);

                //using (StreamReader sr = System.IO.File.OpenText(path))
                //{
                //    string s = "";
                //    while ((s = sr.ReadLine()) != null)
                //    {
                //        txt += s;
                //    }
                //}

                txt = txt.ToUpper().Replace("  ", string.Empty).Trim().Replace("\n", string.Empty).Replace("\t", string.Empty);

                var spectrumStore = new SpectrumStoreModel();
                spectrumStore.CharsCount = txt.Length;

                var store = spectrumStore.Store;

                for (int i = 0; i < txt.Length; i++)
                {
                    if (store.ContainsKey(txt[i]))
                    {
                        store[txt[i]] = store[txt[i]] + 1;
                        spectrumStore.LettersCount++;
                    }
                }

                //sort by keys
                /*var list = spectrumStore.Alphabet;
                list.Sort();

                var spec = new List<int>();
                foreach (var key in list)
                {
                    spec.Add(store[key]);
                }
                spectrumStore.Spectrum = spec;*/
                spectrumStore.Spectrum = new List<int>(store.Values);

                var json = JsonConvert.SerializeObject(spectrumStore);

                return Json(json, JsonRequestBehavior.AllowGet);
            }

            return HttpNotFound();
        }

    }
}