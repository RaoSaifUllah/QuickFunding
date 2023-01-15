using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFunding.Models
{
    public class RegisterResponseMessage
    {
        public string Message { get; set; }
        public bool isSuccess { get; set; }
        public IEnumerable<string> Error { get; set; }
    }
}
