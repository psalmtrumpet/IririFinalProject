using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IririFinalProject.Models
{
    public class PaymentPlan
    {

        public Guid PaymentPlanId { get; set; }
        public string PaymentPlanName { get; set; }
        public double cost { get; set; }
        public string Description { get; set; }
    }
}
