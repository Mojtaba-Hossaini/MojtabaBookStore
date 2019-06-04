using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
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

        public AccountController(IApplicationRoleManager roleManager, IApplicationUserManager userManager, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.signInManager = signInManager;
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
            if (!ModelState.IsValid)
                return View(viewModel);

            var user = new ApplicationUser { UserName = viewModel.UserName, Email = viewModel.Email, PhoneNumber = viewModel.PhoneNumber, RegisterDate = DateTime.Now, IsActive = true };
            IdentityResult result = await userManager.CreateAsync(user, viewModel.Password);

            if (result.Succeeded)
            {
                var role = roleManager.FindByNameAsync("کاربر");
                if (role == null)
                    await roleManager.CreateAsync(new ApplicationRole("کاربر"));

                result = await userManager.AddToRoleAsync(user, "کاربر");
                if (result.Succeeded)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var calBackUrl = Url.Action("ConfirmEmail", "Account", values: new { userId = user.Id, code = code }, Request.Scheme);
                    await emailSender.SendEmailAsync(user.Email, "تایید حساب کاربری - فروشگاه کتاب مجتبی", $"<div dir='rtl' style='font-family:tahoma;font-size:14px'>لطفا با کلیک روی لینک رویه رو ایمیل خود را تایید کنید.  <a href='{HtmlEncoder.Default.Encode(calBackUrl)}'>کلیک کنید</a></div>");

                    return RedirectToAction("Index", "Home", new { id = "ConfirmEmail" });
                }
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(string.Empty, item.Description);
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
            
                if (ModelState.IsValid)
                {
                    var result = await signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.RememberMe, false);
                    if (result.Succeeded)
                        return RedirectToAction("Index", "Home");

                    ModelState.AddModelError(string.Empty, "نام کاربری یا کلمه عبور شما درست نیست");

                }

            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut()
        {
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
    }
}