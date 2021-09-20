using IririFinalProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IririFinalProject.Controllers
{
    public class EventPaystackController : Controller
    {
        private readonly IOptions<Variable> _appSettings;
        public EventPaystackController(IOptions<Variable> appSettings)
        {
            _appSettings = appSettings;
        }



        public IActionResult Index(double amount)
        {
            //var res = JsonConvert.DeserializeObject<LoginModel>(HttpContext.Session.GetString("Priviledges"));
            var res = JsonConvert.DeserializeObject<LoginModel>(HttpContext.Session.GetString("Priviledges"));

            ViewBag.email = res.UserName;
            ViewBag.amount = amount;


            return View();
        }
    }
}
