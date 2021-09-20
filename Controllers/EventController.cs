using IririFinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

using Microsoft.Extensions.Options;

using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace IririFinalProject.Controllers
{
    public class EventController : Controller
    {
        private readonly IOptions<Variable> _appSettings;

        public EventController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }


        public IActionResult Index()
        {

            IEnumerable<EventModel> events = null;
            IEnumerable<EventModel> isTeaserEvents = null;
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

                    events = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    events = Enumerable.Empty<EventModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(_appSettings.Value.host + "api/Event/GetAllTeaserEvent");
                //HTTP GET

                var responseTask = client.GetAsync("GetAllTeaserEvent");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<EventModel>>();
                    readTask.Wait();

                    isTeaserEvents = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    events = Enumerable.Empty<EventModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            ViewBag.events = events;
            ViewBag.isTeaser = isTeaserEvents;


            return View();
        }




        public JsonResult UploadEvent(IFormCollection evenfortModel)
        {
            
            EventModel eventModel = new EventModel();
            HttpResponseMessage result;
            string response = string.Empty;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(_appSettings.Value.host + "api/Event/AddNewEvent");
                var postTask = client.PostAsJsonAsync<EventModel>("AddNewEvent", eventModel);
                postTask.Wait();
                result = postTask.Result;


                if (result.IsSuccessStatusCode)
                {
                    response = "success";
                }
                else
                {

                    var response2 = result.Content.ReadAsStringAsync();
                    string res = response2.Result;
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

