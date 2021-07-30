using IririFinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IririFinalProject.Controllers
{
    public class ViewEventController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public ViewEventController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }
        public IActionResult Index()
        {
            IEnumerable<EventModel> events = null;
            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new Uri(_appSettings.Value.host + "api/Event/GetAllPendingEvents");
                //HTTP GET

                var responseTask = client.GetAsync("GetAllPendingEvents");
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



        public JsonResult PublishEvent(Guid Id)
        {
            HttpResponseMessage result;
            string response = string.Empty;
            using (var client = new HttpClient())
            {



                try
                {

                    client.BaseAddress = new Uri(_appSettings.Value.host + "api/Event/ApproveEvent?Id=" + Id);

                    var postTask = client.GetAsync("ApproveEvent?Id=" + Id);
                    postTask.Wait();
                    result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        response = "success";
                    }
                    else
                    {

                        var response2 = result.Content.ReadAsStringAsync();


                        return Json(new { Url = "Home/Index" });
                    }
                }
                catch (Exception ex)
                {

                    var error = ex.Message + " " + ex.InnerException;
                }

            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return Json(response);


        }
    }
}
