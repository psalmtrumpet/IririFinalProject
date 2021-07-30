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
    public class PastEventController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public PastEventController(IOptions<Variable> appSettings)
        {

            _appSettings = appSettings;
        }
        public IActionResult Index()
        {
            IEnumerable<EventModel> PastEvents = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(_appSettings.Value.host + "api/Event/ViewPastEvents");
                //HTTP GET

                var responseTask = client.GetAsync("ViewPastEvents");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<EventModel>>();
                    readTask.Wait();

                    PastEvents = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    PastEvents = Enumerable.Empty<EventModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }
            ViewBag.PastEvents = PastEvents;
            return View();
        }
    }
}
