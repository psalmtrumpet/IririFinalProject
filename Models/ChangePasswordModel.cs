using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IririFinalProject.Models
{
    public class ChangePasswordModel
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
