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
    public class AnnouncementController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public AnnouncementController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }
        public IActionResult Index()
        {
            return View();
        }



        public JsonResult CreateAnnouncement(Announcement eventModel)
        {

            

            HttpResponseMessage result;
            string response = string.Empty;
            using (var client = new HttpClient())
            {


                client.BaseAddress = new Uri(_appSettings.Value.host + "api/Event/CreateAnnoucement");

                var postTask = client.PostAsJsonAsync<Announcement>("CreateAnnoucement", eventModel);
                postTask.Wait();
                result = postTask.Result;
                // }

                //HTTP POST

                if (result.IsSuccessStatusCode)
                {
                    response = "success";
                }
                else
                {
                    response = "failed";
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return Json(response);
        }


    }
}
