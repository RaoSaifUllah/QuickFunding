using QuickFunding.Models;
using Microsoft.AspNetCore.Identity;

namespace QuickFunding.Services.UserServices
{
    public interface IUserServices
    {
        Task<RegisterResponseMessage> RegisterAsync(RegisterViewModel model);
        Task<LoginResponseMessage> LoginAsync(LoginViewModel model);
        //Task<RegisterResponseMessage> ForgotPasswordAsync(ForgotPasswordViewModel model);
        //Task<CodeVerificationResponse> ValidateResetPasswordCode(string code);
        Task<RegisterResponseMessage> ChangePasswordAsync(ChangePasswordViewModel model);
    }
}
