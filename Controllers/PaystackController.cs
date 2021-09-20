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
    public class PaystackController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public PaystackController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }

        public IActionResult Index(string email)
        {
            ViewBag.email = email;
            return View();
        }


        public async Task<ActionResult> PaymentDetails(PaymentModel Model)
        {
            //var res =  SaveImageAsync(formcollection.Files[0]);


            
            HttpResponseMessage result;
            string response = string.Empty;
            using (var client = new HttpClient())
            {


                client.BaseAddress = new Uri(_appSettings.Value.host + "api/MemberDuePayment/PayMemberDues");

                var postTask = client.PostAsJsonAsync<PaymentModel>("PayMemberDues", Model);
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

            return RedirectToAction("Index", "Paystack");
        }













    }
}
