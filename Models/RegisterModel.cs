using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IririFinalProject.Models
{
    public class RegisterModel
    {

        
        
        public string FirstName { get; set; }

        
        public string LastName { get; set; }

        
        public string Gender { get; set; }

        
        public string MemberAddress { get; set; }

        public string MemberEmail { get; set; }

        public string MemberPhone { get; set; }

        
        public string Occupation { get; set; }

     
        public DateTime DOB { get; set; }
        public string Plan { get; set; }



        //public string Password { get; set; }

   
        //public string ConfirmPassword { get; set; }
    }
}
