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
    public class UpcomingEventController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public UpcomingEventController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }
        public IActionResult Index()
        {
            IEnumerable<EventModel> events = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(_appSettings.Value.host + "api/Event/ViewUpcomingEvents");
                //HTTP GET

                var responseTask = client.GetAsync("ViewUpcomingEvents");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<EventModel>>();
                    readTask.Wait();

                    events = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    events = Enumerable.Empty<EventModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }
            ViewBag.events = events;
            return View();
        }
    }
}
