using Core.Helpers;
using Core.Infrastructure.Email;
using Core.Infrastructure.NotificationService;
using Entities.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcWeb.Areas.Identity.Models;
using Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWeb.Areas.Identity.Controllers
{
    [Area(AreaDefaults.IdentityAreaName)]
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailSender _emailSender;
        private readonly INotificationService _notificationService;

        public AccountController(IAuthenticationService authenticationService, IEmailSender emailSender, INotificationService notificationService)
        {
            _authenticationService = authenticationService;
            _emailSender = emailSender;
            _notificationService = notificationService;
        }

        public IActionResult Login(string returnUrl)
        {
            if (_authenticationService.IsLoggedIn())
                return RedirectToAction("Index", "Home", new { Area = "" });

            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _authenticationService.Login(model.Email, model.Password, model.RememberMe);

            if (string.IsNullOrWhiteSpace(model.ReturnUrl))
                return RedirectToAction("Index", "Home", new { Area = "" });

            return Redirect(model.ReturnUrl);
        }

        public IActionResult Register()
        {
            if (_authenticationService.IsLoggedIn())
                return RedirectToAction("Index", "Home", new { Area = "" });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _authenticationService.Register(new User
            {
                Email = model.Email,
                UserName = model.Email
            }, model.Password);

            _notificationService.SuccessNotification("Başarıyla kayıt oldunuz.");

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> LogOut()
        {
            await _authenticationService.LogOut();

            _notificationService.SuccessNotification("Sistemden başarıyla çıkış yapıldı.");

            return RedirectToAction(nameof(Login));
        }

        public IActionResult ResetPassword()
        {
            if (_authenticationService.IsLoggedIn())
                return RedirectToAction("Index", "Home", new { Area = "" });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var passResetToken = await _authenticationService.GenerateResetPasswordToken(model.Email);

            _emailSender.SendEmail("emrah.atalay@outlook.com", "Şifre Sıfırla", passResetToken);

            _notificationService.SuccessNotification("Şifre sıfırlama kodu mail adresinize gönderilmiştir.");

            return RedirectToAction(nameof(Login));
        }

        public IActionResult ResetPasswordConfirm(string userId, string token)
        {
            return View(new ResetPasswordConfirmViewModel
            {
                UserId = userId,
                Token = token
            });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm(ResetPasswordConfirmViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var passReset = await _authenticationService.ResetPassword(model.UserId, model.Token, model.NewPassword);

            if (!passReset) return View(model);

            _notificationService.SuccessNotification("Şifre sıfırlama işleminiz başarıyla gerçekleşmiştir.");
            return RedirectToAction(nameof(Login));

        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
