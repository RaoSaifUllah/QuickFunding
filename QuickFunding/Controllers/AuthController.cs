
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickFunding.Services.UserServices;
using QuickFunding.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace QuickFunding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserServices _userServices;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(IUserServices userServices, UserManager<IdentityUser> userManager)
        {
            _userServices = userServices;
            _userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.RegisterAsync(model);
                if (result.isSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.LoginAsync(model);
                if (result.isSuccess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }

        //[HttpPost("ForgotPassword")]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _userServices.ForgotPasswordAsync(model);
        //        if (result.isSuccess)
        //        {
        //            return Ok(result);
        //        }
        //        return BadRequest(result);
        //    }
        //    return BadRequest("Email is required");
        //}

        //[HttpPost("ValidateCode")]
        //public async Task<IActionResult> ValidateResetPwdCode(string code)
        //{
        //    if (code != null)
        //    {
        //        var result = await _userServices.ValidateResetPasswordCode(code);
        //        if (result.isSuccess)
        //        {
        //            return Ok(result);
        //        }
        //        return BadRequest(result);
        //    }
        //    return BadRequest("Code is empty");

        //}


        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await _userServices.ChangePasswordAsync(model);
                if (result.isSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }

            return Unauthorized();
        }

        [HttpPost("Contest")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Contest()
        {
            return Ok();
        }
    }
}
