using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFunding.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
        
        [Required]
        public string Token { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
