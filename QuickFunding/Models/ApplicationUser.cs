using Microsoft.AspNetCore.Identity;

namespace QuickFunding.Models
{
	public class ApplicationUser : IdentityUser
	{
		[PersonalData]
		public String FirstName { get; set; }
		[PersonalData]
		public String LastName { get; set; }

	}
}
