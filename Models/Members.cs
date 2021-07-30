using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IririFinalProject.Models
{
    public class Members
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MemberPhone { get; set; }

        public string MemberEmail { get; set; }
        public string Occupation { get; set; }

        public DateTime DOB { get; set; }


    }
}
