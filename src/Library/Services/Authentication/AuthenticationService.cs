using Entities.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContext;

        public AuthenticationService(UserManager<User> userManager,
            SignInManager<User> signInManager, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContext = httpContext;
        }

        public async Task Register(User user, string password)
        {
            user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user, password);

            if (result.Errors.Any())
                throw new Exception(result.Errors.First().Description);
        }

        public async Task Login(string email, string password, bool rememberMe)
        {
            var user = await FindUserByEmailAsync(email);

            if (await _userManager.IsLockedOutAsync(user))
                throw new Exception("Hesabınız bir süreliğine kilitlenmiştir.");

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);

            if (result.Succeeded)
                await _userManager.ResetAccessFailedCountAsync(user);
            else
            {
                await _userManager.AccessFailedAsync(user);
                throw new Exception("Giriş Başarısız.");
            }
        }

        public async Task<User> FindUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                throw new Exception($"Sistemde {userId} kullanıcısı bulunamadı.");

            return user;
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                throw new Exception($"{email} adresi sistemde bulunamadı.");

            return user;
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public bool IsLoggedIn()
        {
            var user = _httpContext.HttpContext.User;
            return _signInManager.IsSignedIn(user);
        }

        public async Task<string> GenerateResetPasswordToken(string email)
        {
            var user = await FindUserByEmailAsync(email);

            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> ResetPassword(string userId, string token, string newPassword)
        {
            var user = await FindUserByIdAsync(userId);

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            return result.Succeeded;
        }
    }
}
