using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models.Auth;

namespace Services.Authentication
{
    public interface IAuthenticationService
    {
        Task Register(User user, string password);
        Task Login(string email, string password, bool rememberMe);
        Task<User> FindUserByIdAsync(string userId);
        Task<User> FindUserByEmailAsync(string email);
        Task LogOut();
        bool IsLoggedIn();
        Task<string> GenerateResetPasswordToken(string email);
        Task<bool> ResetPassword(string userId, string token, string newPassword);
    }
}
