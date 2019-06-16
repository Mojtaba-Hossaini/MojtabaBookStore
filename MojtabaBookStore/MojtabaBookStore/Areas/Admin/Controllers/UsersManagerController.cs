using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MojtabaBookStore.Areas.Identity.Data;
using MojtabaBookStore.Models.ViewModels.UsersManager;
using MojtabaBookStore.Services;
using ReflectionIT.Mvc.Paging;

namespace MojtabaBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersManagerController : Controller
    {
        private readonly IApplicationUserManager userManager;
        private readonly IApplicationRoleManager roleManager;
        private readonly IConvertDate convertDate;
        private readonly IEmailSender emailSender;

        public UsersManagerController(IApplicationUserManager userManager, IApplicationRoleManager roleManager, IConvertDate convertDate, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.convertDate = convertDate;
            this.emailSender = emailSender;
        }
        public async Task<IActionResult> Index(string Msg, int page = 1, int row =10)
        {
            if (Msg == "Success")
                ViewBag.Alert = "عضویت با موفقیت انجام شد";

            if (Msg == "SendEmailSuccess")
                ViewBag.Alert = "ارسال ایمیل با موفقیت انجام شد";

            var pagingModel = PagingList.Create(await userManager.GetAllUsersWithRolesAsync(), row, page);
            return View(pagingModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var user = await userManager.FindUserWithRolesByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var user = await userManager.FindUserWithRolesByIdAsync(id);
            if (user == null)
                return NotFound();

            ViewBag.AllRoles = roleManager.GetAllRoles();

            if(user.BirthDate != null)
                user.PersianBirthDate = convertDate.MiladiToShamsi((DateTime)user.BirthDate, "yyyy/MM/dd");

            return View(user);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsersViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var User = await userManager.FindByIdAsync(viewModel.Id);
                if (User == null)
                    return NotFound();
                else
                {
                    IdentityResult Result;
                    var RecentRoles = await userManager.GetRolesAsync(User);
                    var DeleteRoles = RecentRoles.Except(viewModel.Roles);
                    var AddRoles = viewModel.Roles.Except(RecentRoles);

                    Result = await userManager.RemoveFromRolesAsync(User, DeleteRoles);
                    if (Result.Succeeded)
                    {
                        Result = await userManager.AddToRolesAsync(User, AddRoles);
                        if (Result.Succeeded)
                        {
                            User.FirstName = viewModel.FirstName;
                            User.LastName = viewModel.LastName;
                            User.Email = viewModel.Email;
                            User.PhoneNumber = viewModel.PhoneNumber;
                            User.UserName = viewModel.UserName;
                            User.BirthDate = convertDate.ShamsiToMiladi(viewModel.PersianBirthDate);

                            Result = await userManager.UpdateAsync(User);
                            if (Result.Succeeded)
                            {
                                ViewBag.AlertSuccess = "ذخیره تغییرات با موفقیت انجام شد.";
                            }
                        }
                    }

                    if (Result != null)
                    {
                        foreach (var item in Result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
            }

            ViewBag.AllRoles = roleManager.GetAllRoles();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();
            var User = await userManager.FindByIdAsync(id);
            if (User == null)
                return NotFound();
            else
                return View(User);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> Deleted(string id)
        {
            if (id == null)
                return NotFound();
            var User = await userManager.FindByIdAsync(id);
            if (User == null)
                return NotFound();
            else
            {
                var Result = await userManager.DeleteAsync(User);
                if (Result.Succeeded)
                    return RedirectToAction("Index");
                else
                    ViewBag.AlertError = "در حذف اطلاعات خطایی رخ داده است.";

                return View(User);
            }
        }

        public async Task<IActionResult> SendEmail(string[] emails, string subject, string message)
        {
            if (emails != null)
            {
                for (int i = 0; i < emails.Length; i++)
                {
                    await emailSender.SendEmailAsync(emails[i], subject, message);
                }
                return RedirectToAction("Index", new { Msg = "SendEmailSuccess" }); ;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeLockOutEnable(string userId, bool status)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            await userManager.SetLockoutEnabledAsync(user, status);
            return RedirectToAction("Details", new { id = userId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUserAccount(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(20));
            return RedirectToAction("Details", new { id = userId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnLockUserAccount(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            await userManager.SetLockoutEndDateAsync(user, null);
            return RedirectToAction("Details", new { id = userId });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActiveOrDeactiveUser(string userId, bool status)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            user.IsActive = status;
            await userManager.UpdateAsync(user);
            return RedirectToAction("Details", new { id = userId });

        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();
            var viewModel = new UserResetPasswordViewModel
            {
                Id = userId,
                UserName = user.UserName,
                Email = user.Email
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(UserResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(viewModel.Id);
                if (user == null)
                    return NotFound();

                await userManager.RemovePasswordAsync(user);
                await userManager.AddPasswordAsync(user, viewModel.NewPassword);
                ViewBag.AlertSuccess = "رمز عبور کاربر با موفقیت ریست شد";
                viewModel.UserName = user.UserName;
                viewModel.Email = user.Email;

            }
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeTwoFactorEnalbed(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            if (user.TwoFactorEnabled)
                user.TwoFactorEnabled = false;
            else
                user.TwoFactorEnabled = true;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return RedirectToAction("Details", new { id = userId });
            


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmailConfirmed(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            if (user.EmailConfirmed)
                user.EmailConfirmed = false;
            else
                user.EmailConfirmed = true;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return RedirectToAction("Details", new { id = userId });



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePhoneNumberConfirmed(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            if (user.PhoneNumberConfirmed)
                user.PhoneNumberConfirmed = false;
            else
                user.PhoneNumberConfirmed = true;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return RedirectToAction("Details", new { id = userId });



        }








    }
}