using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
            return View();
        }

        public ActionResult FileUploadOnServer(HttpPostedFileBase file)
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

        /*public void FileUploadOnPc(string filename)
        {
            //return RedirectToAction(Server.MapPath("~/Files/")+filename);
            Response.ContentType = "APPLICATION/OCTET-STREAM";
            String Header = "Attachment; Filename=XMLFile.xml";
            Response.AppendHeader("Content-Disposition", Header);
            System.IO.FileInfo Dfile = new System.IO.FileInfo(Server.MapPath("FileArchieve/Flowers/Flowers.png"));
            Response.WriteFile(Dfile.FullName);
            //Don't forget to add the following line
            Response.End();
        }*/

        public ActionResult Spectrum(string filename)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Files/" + filename);
                string txt = System.IO.File.ReadAllText(path);

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

                spectrumStore.Spectrum = new List<int>(store.Values);

                var json = JsonConvert.SerializeObject(spectrumStore);

                return Json(json, JsonRequestBehavior.AllowGet);
            }

            return HttpNotFound();
        }

        public ActionResult AffineFileCrypting(string filename, bool enc, int a, int b)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Files/" + filename);
                AffineModel affine = new AffineModel(a, b);
                FileModel output = new FileModel();

                //true =  encrypting
                if (enc)
                {
                    output = affine.EncryptFile(path);
                }
                else
                {
                    output = affine.DecryptFile(path);
                }

                var json = JsonConvert.SerializeObject(output); ;

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        public ActionResult BitFileCrypting(string filename, int a, int b, bool act)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Files/" + filename);
                BitModel bit = new BitModel(a, b);

                Debug.WriteLine("The path is " + path);

                var output = bit.Crypting(path, act);

                var json = JsonConvert.SerializeObject(output); ;

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        public ActionResult VigenereFileCrypting(string filename, bool act, string key)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Files/" + filename);
                var vigenere = new VigenereModel(key.ToUpper().Trim());

                if (!vigenere.VerifyKey())
                    return new HttpStatusCodeResult(400, "Wrong Key: symbols in key are incorrect");

                var output = vigenere.Crypting(path, act);

                var json = JsonConvert.SerializeObject(output); ;

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        public ActionResult HillFileCrypting(string filename, bool act, int[] key)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Files/" + filename);
                var hill = new HillModel(new [,]{{key[0], key[1]}, { key[2], key[3]}});

                var output = hill.CryptFile(path, act);

                var json = JsonConvert.SerializeObject(output); ;

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        public ActionResult RSAFileCrypting(string filename, bool act, int[] key)
        {
            if (ModelState.IsValid)
            {
                var path = Server.MapPath("~/Files/" + filename);
                RSAModel rsa = new RSAModel(key);
                FileModel output = new FileModel();

                //true =  encrypting
                if (act)
                {
                    output = rsa.EncryptFile(path);
                }
                else
                {
                    output = rsa.DecryptFile(path);
                }

                var json = JsonConvert.SerializeObject(output); ;

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        public ActionResult RSAGenerateSessionKey(int p, int q)
        {
            if (ModelState.IsValid)
            {
                var key = RSAModel.SessionKeys(p, q);
                var json = JsonConvert.SerializeObject(key); ;

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }
    }
}