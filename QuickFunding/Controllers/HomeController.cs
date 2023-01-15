using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickFunding.Data;
using QuickFunding.Models;
using System.Diagnostics;

namespace QuickFunding.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }
        [Authorize]
        public  IActionResult Authentication()
        {

            return View();
        }
        [Authorize]
        public IActionResult OrganiserProfile()
        {
            return View();
        }
		public IActionResult CreateEvent()
		{
			return View();
		}
		public IActionResult Errors()
		{
			return View();
		}
      //  [Route("pages/contact-us")]
		public IActionResult ContactUs()
		{
			return View();
		}
		public IActionResult AboutUs()
		{
			return View();
		}
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}