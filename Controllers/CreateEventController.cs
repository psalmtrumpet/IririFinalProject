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
    public class CreateEventController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public CreateEventController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult UploadEvent(IFormCollection evenfortModel)
        {
            var base64 = "";
            EventModel eventModel = new EventModel();
            eventModel.EventTitle = evenfortModel["title"];
            eventModel.EventVenue = evenfortModel["venue"];
            eventModel.Amount = double.Parse( evenfortModel["amount"]);
            eventModel.EventDescription = evenfortModel["description"];
            eventModel.Date = DateTime.Parse(evenfortModel["date"]);
         //   eventModel.EventImage = evenfortModel.Files;
            foreach (var file in evenfortModel.Files)
            {
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
            }
            // Model.BankName = formcollection["BankName"];
            eventModel.EventBase64 = base64;


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


        public static string LoadBase64(string base64)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(base64);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return base64ImageRepresentation;
        }
    }
}
