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
    public class AdminDuesController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public AdminDuesController(IOptions<Variable> appSettings)
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

            ViewBag.plans = paymentPlans;
            return View();
        }


        public IActionResult Modal(Guid id)
        {
            IEnumerable<PaymentPlan> paymentPlans = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(_appSettings.Value.host + "GetPaymentPlanById?id="+ id);
                //HTTP GET
                //GetPaymentPlanById?id=419e2577-4ffe-401f-8e3c-08d987083c34

                var responseTask = client.GetAsync("GetPaymentPlanById?id=" + id);
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
                ViewBag.dues = paymentPlans;
                var confirm = ViewBag.dues[0];
                ViewBag.paymentPlanName = confirm.PaymentPlanName;
                ViewBag.paymentPlancost = confirm.cost;
                ViewBag.paymentId = confirm.PaymentPlanId;
                return PartialView("PartialDues");
            }
        }

   
        public JsonResult Update(Guid PaymentPlanId, string PaymentPlanName, double cost)
        {
            //string response = string.Empty;
            PaymentPlan Model = new PaymentPlan();
            Model.cost = cost;
            Model.PaymentPlanId = PaymentPlanId;
            Model.PaymentPlanName = PaymentPlanName;
            HttpResponseMessage result;
            string response = string.Empty;
            using (var client = new HttpClient())
            {


                client.BaseAddress = new Uri(_appSettings.Value.host + "EditPaymentPlan");

                var postTask = client.PutAsJsonAsync<PaymentPlan>("EditPaymentPlan", Model);
                postTask.Wait();
                result = postTask.Result;

                //HTTP POST

                if (result.IsSuccessStatusCode)
                {
                    response = "success";
                }
                else
                {
                    response = "failed";
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return Json(response);
        }
    }
}
