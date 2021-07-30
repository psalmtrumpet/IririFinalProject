using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IririFinalProject.Models
{
    public class LoginResponse
    {
        public string token { get; set; }
        public string exactRole { get; set; }
        public IList<string> role { get; set; }
        public string memberId { get; set; }
    }
}
