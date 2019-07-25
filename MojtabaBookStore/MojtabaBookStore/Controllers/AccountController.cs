using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MojtabaBookStore.Areas.Identity.Data;
using MojtabaBookStore.Models.ViewModels.AccountViewModel;
using MojtabaBookStore.Services;

namespace MojtabaBookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApplicationRoleManager roleManager;
        private readonly IApplicationUserManager userManager;
        private readonly IEmailSender emailSender;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ConvertDate convertDate;

        public AccountController(IApplicationRoleManager roleManager, IApplicationUserManager userManager, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager, ConvertDate convertDate)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.signInManager = signInManager;
            this.convertDate = convertDate;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                DateTime BirthDateMiladi = convertDate.ShamsiToMiladi(viewModel.BirthDate);

                var user = new ApplicationUser { UserName = viewModel.UserName, Email = viewModel.Email, PhoneNumber = viewModel.PhoneNumber, RegisterDate = DateTime.Now, IsActive = true, BirthDate = BirthDateMiladi };
                IdentityResult result = await userManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    var role = await roleManager.FindByNameAsync("کاربر");
                    if (role == null)
                        await roleManager.CreateAsync(new ApplicationRole("کاربر"));

                    result = await userManager.AddToRoleAsync(user, "کاربر");
                    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.DateOfBirth, BirthDateMiladi.ToString("MM/dd")));

                    if (result.Succeeded)
                    {
                        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", values: new { userId = user.Id, code = code }, protocol: Request.Scheme);

                        await emailSender.SendEmailAsync(viewModel.Email, "تایید ایمیل حساب کاربری - سایت میزفا", $"<div dir='rtl' style='font-family:tahoma;font-size:14px'>لطفا با کلیک روی لینک رویه رو ایمیل خود را تایید کنید.  <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>کلیک کنید</a></div>");

                        return RedirectToAction("Index", "Home", new { id = "ConfirmEmail" });
                    }
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }

            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return RedirectToAction("Index", "Home");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Unable to find any User with ID '{userId}' ");
            var result = await userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
                throw new InvalidOperationException($"در تایید ایمیل کاربری با آی دی '{userId}' مشکلی به وجود آمد ");
            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel viewModel)
        {


            if (Captcha.ValidateCaptchaCode(viewModel.CaptchaCode, HttpContext))
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByNameAsync(viewModel.UserName);
                    if (user == null)
                        return NotFound();
                    if (user.IsActive)
                    {
                        var result = await signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.RememberMe, true);
                        if (result.Succeeded)
                            return RedirectToAction("Index", "Home");
                        if (result.IsLockedOut)
                        {
                            ModelState.AddModelError(string.Empty, "حساب کاربری شما به علت تلاش های ناموفق در ورود به مدت ۲۰ دقیقه قفل میباشد. بعدا تلاش کنید");
                            return View();

                        }
                        if (result.RequiresTwoFactor)
                            return RedirectToAction("Sendcode", new { rememeberMe = viewModel.RememberMe });

                    }



                    ModelState.AddModelError(string.Empty, "نام کاربری یا کلمه عبور شما درست نیست");

                }
            }
            else
                ModelState.AddModelError(string.Empty, "کد امنیتی وارد شده درست نیست");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut()
        {
            var user = await userManager.GetUserAsync(User);
            user.LastVisitDateTime = DateTime.Now;
            await userManager.UpdateAsync(user);
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage()
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(viewModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "ایمیل شما صحیح نمی باشد.");
                    return View();
                }

                if (!await userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "لطفا با تایید ایمیل حساب کاربری خود را فعال کنید.");
                    return View();
                }

                var Code = await userManager.GeneratePasswordResetTokenAsync(user);
                var CallbackUrl = Url.Action("ResetPassword", "Account", values: new { Code }, protocol: Request.Scheme);
                await emailSender.SendEmailAsync(viewModel.Email, "بازیابی کلمه عبور", $"<p style='font-family:tahoma;font-size:14px'>برای بازنشانی کلمه عبور خود <a href='{HtmlEncoder.Default.Encode(CallbackUrl)}'>اینجا کلیک کنید</a></p>");

                return RedirectToAction("ForgetPasswordConfirmation");
            }

            return View();
        }

        [HttpGet]
        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
                return NotFound();
            else
            {
                var ViewModel = new ResetPasswordViewModel { Code = code };
                return View(ViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var user = await userManager.FindByEmailAsync(viewModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "ایمیل شما صحیح نمی باشد.");
                    return View();
                }
                var Result = await userManager.ResetPasswordAsync(user, viewModel.Code, viewModel.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View();
            }
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public async Task<IActionResult> SendCode(bool rememberMe)
        {
            var factorOptoins = new List<SelectListItem>();
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
                return NotFound();

            var userFactors = await userManager.GetValidTwoFactorProvidersAsync(user);
            foreach (var item in userFactors)
            {
                if (item == "Authenticator")
                    factorOptoins.Add(new SelectListItem { Text = "اپلیکیشن کد ساز", Value = item });
                else
                    factorOptoins.Add(new SelectListItem { Text = (item == "Email" ? "ایمیل" : "ارسال پیامک"), Value = item });
            }
            
            return View(new SendCodeViewModel { Providers = factorOptoins, RememberMe = rememberMe });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
                return NotFound();

            if (viewModel.SelectedProvider != "Authenticator")
            {
                var code = await userManager.GenerateTwoFactorTokenAsync(user, viewModel.SelectedProvider);
                if (string.IsNullOrWhiteSpace(code))
                    return View("Error");

                var message = "<p style='direction:rtl;font-size:14px;font-family:tahoma'>کد اعتبارسنجی شما :" + code + "</p>";
                if (viewModel.SelectedProvider == "Email")
                    await emailSender.SendEmailAsync(user.Email, "کد اعتبار سنجی فروشگاه کتاب مجتبی", message);

                return RedirectToAction("VerifyCode", new { provider = viewModel.SelectedProvider, rememberMe = viewModel.RememberMe });
            }
            else
                return RedirectToAction("LoginWith2fa", new { rememberMe = viewModel.RememberMe });
            
        }

        [HttpGet]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe)
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
                return NotFound();

            return View(new LoginWith2faViewModel { RememberMe = rememberMe});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
                return NotFound();
            var authenticationCode = viewModel.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
            var result = await signInManager.TwoFactorAuthenticatorSignInAsync(authenticationCode, viewModel.RememberMe, viewModel.RememberMachine);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else if (result.IsLockedOut)
                ModelState.AddModelError(string.Empty, "حساب کاربری شما به علت تلاش های ناموفق به مدت ۲۰ دقیقه قفل میباشد ");
            else
                ModelState.AddModelError(string.Empty, "کد وارد شده درست نیست");

            return View(viewModel);
                

        }

        [HttpGet]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe)
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
                return NotFound();

            return View(new VerifyCodeViewModel { Provider = provider, RememberMe = rememberMe });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var result = await signInManager.TwoFactorSignInAsync(viewModel.Provider, viewModel.Code, viewModel.RememberMe, viewModel.RememberBrowser);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else if (result.IsLockedOut)
                ModelState.AddModelError(string.Empty, "حساب کاربری شما به مدت ۲۰ دقیقه قفل میباشد");
            else
                ModelState.AddModelError(string.Empty, "کد وارد شده درست نیست");

            return View(viewModel);




        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();
            UserSidebarViewModel userSidebar = UpdateUserSideBar(user);

            return View(new ChangePasswordViewModel { UserSidebar = userSidebar });
        }

        private static UserSidebarViewModel UpdateUserSideBar(ApplicationUser user)
        {
            return new UserSidebarViewModel
            {
                FullName = user.FirstName + " " + user.LastName,
                LastVisit = user.LastVisitDateTime,
                RegisterDate = user.RegisterDate,
                Image = user.Image
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var changePassResult = await userManager.ChangePasswordAsync(user, viewModel.OldPassword, viewModel.NewPassword);
                if (changePassResult.Succeeded)
                    ViewBag.Alert = "کلمه عبور شما با موفقیت تغییر کرد";
                else
                {
                    foreach (var item in changePassResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }
                }
            }
            UserSidebarViewModel userSidebar = UpdateUserSideBar(user);

            viewModel.UserSidebar = userSidebar;
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LoginWithRecoveryCode()
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
                return NotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
                return NotFound();

            var recoveryCode = viewModel.RecoveryCode.Replace(" ", string.Empty);
            var result = await signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            else if (result.IsLockedOut)
                ModelState.AddModelError(string.Empty, "حساب کاربری شما به مدت 20 دقیقه به دلیل تلاش های ناموفق قفل شد.");

            else
                ModelState.AddModelError(string.Empty, "کد بازیابی شما نامعتبر است.");

            return View(viewModel);
        }

        public IActionResult AccessDenied(string returnUrl = null)
        {
            return View();
        }

    }
}