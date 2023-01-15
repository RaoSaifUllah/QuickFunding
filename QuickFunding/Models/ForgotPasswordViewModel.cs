using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuickFunding.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [StringLength(maximumLength:50,ErrorMessage ="Email is required",MinimumLength =6)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
