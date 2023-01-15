using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFunding.Models
{
    public class LoginResponseMessage
    {
        public string Auth { get; set; }
        public bool isSuccess { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
