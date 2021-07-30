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
    public class AdminViewEventController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public AdminViewEventController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }
        public IActionResult Index()
        {

            IEnumerable<Announcement> announcements = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(_appSettings.Value.host + "api/Event/ViewAnnoucement");
                //HTTP GET

                var responseTask = client.GetAsync("ViewAnnoucement");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Announcement>>();
                    readTask.Wait();

                    announcements = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    announcements = Enumerable.Empty<Announcement>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            ViewBag.announcement = announcements;
            return View();
        }
    }
}
