using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IririFinalProject.Models
{
    public class EventModel
    {
        public string EventId { get; set; }
        public string EventTitle { get; set; }

        public DateTime Date { get; set; }

        public string EventVenue { get; set; }


        public string EventBase64 { get; set; }

        public string EventDescription { get; set; }

       
       // public IFormFileCollection EventImage { get; set; }
        public string EventPicture { get; set; }

        public double Amount { get; set; }



   
    }
}
