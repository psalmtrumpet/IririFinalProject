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
    public class GetApproveMembersController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public GetApproveMembersController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }
        public IActionResult Index()
        {
            IEnumerable<Members> members = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(_appSettings.Value.host + "api/Admin/GetApprovedRegistrations");
                //HTTP GET

                var responseTask = client.GetAsync("GetApprovedRegistrations");
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
    }
}
