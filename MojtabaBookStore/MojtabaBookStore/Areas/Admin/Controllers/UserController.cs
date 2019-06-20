using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MojtabaBookStore.Areas.Identity.Data;
using MojtabaBookStore.Models.ViewModels.UserViewModel;

namespace MojtabaBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IApplicationUserManager userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UrlEncoder urlEncoder;

        public UserController(IApplicationUserManager userManager, SignInManager<ApplicationUser> signInManager, UrlEncoder urlEncoder)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.urlEncoder = urlEncoder;
        }
        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            return View(await LoadSharedKeyAndQrCodeUriAsync(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel viewModel)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();
            if (!ModelState.IsValid)
                return View(await LoadSharedKeyAndQrCodeUriAsync(user));

            var verificationCode = viewModel.Code.Replace(" ", string.Empty).Replace("-", string.Empty);
            var is2faTokenValid = await userManager.VerifyTwoFactorTokenAsync(user, userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);
            if (!is2faTokenValid)
            {
                ModelState.AddModelError(string.Empty, "کد اعتبار سنجی دو مرحله ای معتبر نیست");
                return View(await LoadSharedKeyAndQrCodeUriAsync(user));
            }

            await userManager.SetTwoFactorEnabledAsync(user, true);
            

            if (await userManager.CountRecoveryCodesAsync(user) == 0)
            {
                var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                ViewBag.Alert = "اپلیکیشن احراز هویت شما تایید شده است";
                return View("ShowRecoveryCodes", recoveryCodes);
            }
            else
                return RedirectToAction("TwoFactorAuthentication", new { alert = "success" });

        }

        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication(string alert)
        {
            if(alert != null)
                ViewBag.Alert = "اپلیکیشن احراز هویت شما تایید شده است";

            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            TwoFactorAuthenticationViewModel viewModel = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await userManager.GetAuthenticatorKeyAsync(user) != null,
                RecoveryCodesLeft = await userManager.CountRecoveryCodesAsync(user),
                Is2faEnabled = await userManager.GetTwoFactorEnabledAsync(user)
            };
            return View(viewModel);
        }

        public async Task<EnableAuthenticatorViewModel> LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user)
        {
            var unFormattedKey = await userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unFormattedKey))
            {
                await userManager.ResetAuthenticatorKeyAsync(user);
                unFormattedKey = await userManager.GetAuthenticatorKeyAsync(user);
            }

            EnableAuthenticatorViewModel viewModel = new EnableAuthenticatorViewModel
            {
                AuthenticatorUri = GenerateQrCodeUri(unFormattedKey, user.Email),
                SharedKey = FormatKey(unFormattedKey),
            };

            return viewModel;
        }

        public string FormatKey(string unFormattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;

            while (currentPosition +4 < unFormattedKey.Length)
            {
                result.Append(unFormattedKey.Substring(currentPosition, 4));
                result.Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unFormattedKey.Length)
                result.Append(currentPosition);

            return result.ToString().ToLowerInvariant();

        }

        public string GenerateQrCodeUri(string unFormattedKey, string email)
        {
            string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
            return (string.Format(AuthenticatorUriFormat, urlEncoder.Encode("MojtabaBookStore"), urlEncoder.Encode(email), unFormattedKey));

        }

        [HttpGet]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var IsTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user);
            if (!IsTwoFactorEnabled)
                throw new InvalidOperationException("امکان ایجاد کد بازیابی وجود ندارد چون احراز هویت دو مرحله ای فعال نیست.");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("GenerateRecoveryCodes")]
        public async Task<IActionResult> GenerateRecovery()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var IsTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user);
            if (!IsTwoFactorEnabled)
                throw new InvalidOperationException("امکان ایجاد کد بازیابی وجود ندارد چون احراز هویت دو مرحله ای فعال نیست.");

            var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            return View("ShowRecoveryCodes", recoveryCodes.ToArray());

        }

        [HttpGet]
        public async Task<IActionResult> ResetAuthenticator()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ResetAuthenticator")]
        public async Task<IActionResult> ResetAuthenticatorApp()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            await userManager.SetTwoFactorEnabledAsync(user, false);
            await userManager.ResetAuthenticatorKeyAsync(user);
            await signInManager.RefreshSignInAsync(user);
            return RedirectToAction("EnableAuthenticator");
        }
    }
}