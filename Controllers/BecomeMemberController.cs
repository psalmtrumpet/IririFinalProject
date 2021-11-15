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
    public class BecomeMemberController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public BecomeMemberController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }
        public IActionResult Index()
        {
            IEnumerable<PaymentPlan> paymentPlans = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(_appSettings.Value.host + "ViewPaymentPlans");
                //HTTP GET

                var responseTask = client.GetAsync("ViewPaymentPlans");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<PaymentPlan>>();
                    readTask.Wait();

                    paymentPlans = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    paymentPlans = Enumerable.Empty<PaymentPlan>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }


            }

          //  var plan = paymentPlans;
            ViewBag.dues = paymentPlans;
            ViewBag.plan1 = ViewBag.dues[0].cost;
            ViewBag.plan2 = ViewBag.dues[1].cost;
            ViewBag.plan3 = ViewBag.dues[2].cost;


            return View();
        }
    }
}
