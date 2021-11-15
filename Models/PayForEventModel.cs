using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IririFinalProject.Models
{
    public class PayForEventModel
    {
        public string EventPaymentPlanName { get; set; }
        public string email { get; set; }
        public string paymentReference { get; set; }
        public decimal amount { get; set; }
    }
}
