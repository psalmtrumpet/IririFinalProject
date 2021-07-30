using IririFinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IririFinalProject.Controllers
{
    public class AddGalleryController : Controller
    {
        private readonly IOptions<Variable> _appSettings;

        public AddGalleryController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }

        public IActionResult Index()
        {
            return View();
        }


        public JsonResult UploadGallery(IFormCollection galleryModel)
        {
            var base64 = "";
          
            List<string> mm = new List<string>();
            foreach (var file in galleryModel.Files)
            {
                //string mms = "";
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string convertedImage = Convert.ToBase64String(fileBytes);
                        base64 = convertedImage;
                    }
                }
                mm.Add(base64);
            }
          
            GalleryModel gallery = new GalleryModel();

            gallery.Description = galleryModel["Title"];
            gallery.Event = galleryModel["EventName"];
            gallery.Description = galleryModel["Description"];
            gallery.base64 = mm;

            HttpResponseMessage result;
            string response = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri( _appSettings.Value.host + "api/Event/UploadImageGallery");
                var postTask = client.PostAsJsonAsync<GalleryModel>("UploadImageToGallery", gallery);
                postTask.Wait();
                result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    response = "success";
                }
                else
                {

                    var response2 = result.Content.ReadAsStringAsync();
                    string res = response2.Status.ToString();
                    string[] words = res.Split(',');
                    var splitedword = words[1];
                    var splitedcolumn = splitedword.Split(":");
                    var message = splitedcolumn[1];
                    var message2 = message.Replace("\"", "");
                    response = message2;

                    return Json(new { Url = "Home/Index" });
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return Json(response);
        }




    }
}
