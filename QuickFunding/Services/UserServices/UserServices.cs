using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using QuickFunding.Config;
using QuickFunding.Data;
using QuickFunding.Models;

namespace QuickFunding.Services.UserServices
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserServices(SignInManager<IdentityUser> signInManager, ApplicationDbContext context, UserManager<IdentityUser> userManager, IOptionsMonitor<JwtConfig> optionsMonitor, IEmailSender emailSender)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _context = context;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public async Task<RegisterResponseMessage> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
                return new RegisterResponseMessage
                {
                    Message = "ConfirmPassword not matched",
                    isSuccess = false
                };
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return new RegisterResponseMessage
                {
                    Message = "Password Reset Successfully",
                    isSuccess = true,
                };
            }
            return new RegisterResponseMessage
            {
                Message = "Reset password failed",
                isSuccess = false,
                Error = result.Errors.Select(x => x.Description)
            };
        }

        //public async Task<RegisterResponseMessage> ForgotPasswordAsync(ForgotPasswordViewModel model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist or is not confirmed
        //        return new RegisterResponseMessage
        //        {
        //            Message = "Please check your email for passsword reset link",
        //            isSuccess = true
        //        };
        //    }

        //    Random random = new Random();
        //    var code = random.Next(10000000, 99999999).ToString();
        //    ResetPasswordCode pwdReset = new ResetPasswordCode
        //    {
        //        Code = code,
        //        Status = false,
        //        UserId = user.Id,
        //        ValidTill = DateTime.Now.AddMinutes(5)
        //    };
        //    _context.ResetPasswordCodes.Add(pwdReset);
        //    await _context.SaveChangesAsync();

        //    await _emailSender.SendEmailAsync(user.Email, "Reset Password", $"Password Reset code: {code}");

        //    return new RegisterResponseMessage
        //    {
        //        Message = "Please check your email for passsword reset link",
        //        isSuccess = true
        //    };
        //}

        public async Task<LoginResponseMessage> LoginAsync(LoginViewModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser == null)
            {
                return new LoginResponseMessage
                {
                    Auth = "No user exist with given email",
                    isSuccess = false
                };
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, model.Password);
            var signinResult = await _signInManager.CreateUserPrincipalAsync(existingUser);
            if (!isCorrect)
            {
                return new LoginResponseMessage
                {
                    Auth = "Invalid username or password",
                    isSuccess = false
                };
            }
            //if (!signinResult.IsInRole("Verify"))
            //{
            //    return new LoginResponseMessage
            //    {
            //        Auth = "This operation required verifier level authorization. Access Denied",
            //        isSuccess = false
            //    };
            //}
            var jwtToken = GenerateJwtToken(existingUser);

            return new LoginResponseMessage
            {
                Auth = jwtToken,
                isSuccess = true
            };
        }

        public async Task<RegisterResponseMessage> RegisterAsync(RegisterViewModel model)
        {
            if (model == null)
                throw new NullReferenceException("All properties are required");

            //ToDo: Verify Organizer Code Before Register User
            

            if (model.Password != model.ConfirmPassword)
            {
                return new RegisterResponseMessage
                {
                    Message = "ConfirmPassword dosn't match Password",
                    isSuccess = false,
                };
            }


            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //await _userManager.AddClaimAsync(user, new Claim("OrgCode", model.OrgCode));
                //await _userManager.AddToRoleAsync(user, "Verify");
                //ToDo: Implement User Email Verification using Sendgrid
                return new RegisterResponseMessage
                {
                    Message = "User Created Successfully",
                    isSuccess = true,
                };
            }

            return new RegisterResponseMessage
            {
                Message = "User not created",
                isSuccess = false,
                Error = result.Errors.Select(e => e.Description)
            };
        }

        //public async Task<CodeVerificationResponse> ValidateResetPasswordCode(string Code)
        //{
        //    var resetPwd = await _context.ResetPasswordCodes.FirstAsync(x => x.Code == Code);
        //    var currentTime = DateTime.Now;
        //    if (resetPwd == null || resetPwd.Status == true || resetPwd.ValidTill < currentTime)
        //    {
        //        return new CodeVerificationResponse
        //        {
        //            Message = "Invalid Code",
        //            isSuccess = false,
        //        };
        //    }
        //    var user = await _userManager.FindByIdAsync(resetPwd.UserId);
        //    if (user == null)
        //    {
        //        return new CodeVerificationResponse
        //        {
        //            Message = "Something went wrong. Please try again",
        //            isSuccess = false,
        //        };
        //    }
        //    resetPwd.Status = true;
        //    _context.ResetPasswordCodes.Update(resetPwd);
        //    await _context.SaveChangesAsync();

        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //    return new CodeVerificationResponse
        //    {
        //        Message = "Code Verified",
        //        isSuccess = true,
        //        Token = token,
        //        Email = user.Email,
        //    };
        //}


        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    //new Claim("UserId",user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
