using IririFinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

using Microsoft.Extensions.Options;

using System.Net.Http;
using System.Threading.Tasks;

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

