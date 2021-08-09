using IririFinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IririFinalProject.Controllers
{
    public class MyGalleryController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public MyGalleryController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }
        public IActionResult Index()
        {
            IEnumerable<MyGallery> gallery = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(_appSettings.Value.host + "api/Event/ViewGallery");
                //HTTP GET

                var responseTask = client.GetAsync("Event/ViewGallery");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<MyGallery>>();
                    readTask.Wait();

                    gallery = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    gallery = Enumerable.Empty<MyGallery>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }
            ViewBag.gallery = gallery;
            return View();
        }


     




    }
}
