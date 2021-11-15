using IririFinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IririFinalProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public LoginController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }

        public IActionResult Index()
        {
            return View();
        }


        public JsonResult Authenticate(LoginModel loginModel)
        {
            //var email = loginModel.Email;
            //var password = loginModel.Password;
            LoginResponse priviledges = null;
            var status = string.Empty;
            string response = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.Value.host + "api/MemberUser/Login");

                var responseTask = client.PostAsJsonAsync<LoginModel>("Login", loginModel);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<LoginResponse>();
                    readTask.Wait();
                    priviledges = readTask.Result;
                    HttpContext.Session.SetString("Priviledges", JsonConvert.SerializeObject(loginModel));
                   

                    //  var res = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("Priviledges"));
                    var ss = string.Empty;
                    var memberId = priviledges.memberId;
                 //   HttpContext.Session.SetString("MemberId", JsonConvert.SerializeObject(memberId));
                    response = priviledges.role[0];

                    //if (response =="MEMBER")
                    //{
                    //    return RedirectToAction("Index", "UserDashboard");
                    //}
                    //else
                    //{
                    //    return RedirectToAction("Index", "Dashboard");
                    //}



                }
                else //web api sent error response 
                {
                    //log response status here..

                    priviledges = null;
                    var response2 = result.Content.ReadAsStringAsync();
                    string res = response2.Result;
                    //  return RedirectToAction("Index", "Login");
                    string words = res.Replace("{ ", "");
                    string words2 = res.Replace("}", "");
                    string words23 = res.Replace("}", "");
                    //var splitedword = words[1];
                    //var splitedcolumn = splitedword.Split(":");
                    //var message = splitedcolumn[1];
                    //var message2 = message.Replace("\"", "");
                    //  response = message2;
                    response = "failed";

                    // ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");

                }


                
            }


            return Json(response);
        }




    }
}
