using IririFinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IririFinalProject.Controllers
{
    public class MemberController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public MemberController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }
        public IActionResult Index()

        {
            IEnumerable<Members> members = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(_appSettings.Value.host + "api/Admin/GetPendingRegistrations");
                //HTTP GET

                var responseTask = client.GetAsync("GetPendingRegistrations");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Members>>();
                    readTask.Wait();

                    members = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    members = Enumerable.Empty<Members>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

            ViewBag.members = members;
            return View();
        }



        public   JsonResult ApproveMember(string email)
        {
            HttpResponseMessage result;
            string response = string.Empty;
            using (var client = new HttpClient())
            {



                try
                {

                    client.BaseAddress = new Uri(_appSettings.Value.host + "api/Admin/ApproveMember?email=" + email);
                 
                    var postTask = client.GetAsync("ApproveMember?email=" + email);
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
