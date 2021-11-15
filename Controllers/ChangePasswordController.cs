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
    public class ChangePasswordController : Controller
    {
        private readonly IOptions<Variable> _appSettings;

        public ChangePasswordController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }
        public IActionResult Index()
        {
           // var res = JsonConvert.DeserializeObject<LoginModel>(HttpContext.Session.GetString("Priviledges"));
          //  ViewBag.email = res.UserName;
            return View();
        }



        public async Task<ActionResult> Reset(ChangePasswordModel loginModel)
        {
            
            var res = JsonConvert.DeserializeObject<LoginModel>(HttpContext.Session.GetString("Priviledges"));
            loginModel.Email = res.UserName;
            HttpResponseMessage result;
            string response = string.Empty;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(_appSettings.Value.host + "api/MemberUser/ChangePassword");

                var postTask = client.PostAsJsonAsync<ChangePasswordModel>("ChangePassword", loginModel);
                postTask.Wait();
                result = postTask.Result;


                if (result.IsSuccessStatusCode)
                {
                    response = "success";
                }
                else
                {
                 

                    return Json(new { Url = "Home/Index" });
                }
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
