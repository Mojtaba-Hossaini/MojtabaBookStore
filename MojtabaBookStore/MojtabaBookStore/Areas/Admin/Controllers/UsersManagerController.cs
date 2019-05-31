using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MojtabaBookStore.Areas.Identity.Data;
using ReflectionIT.Mvc.Paging;

namespace MojtabaBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersManagerController : Controller
    {
        private readonly IApplicationUserManager userManager;

        public UsersManagerController(IApplicationUserManager userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index(string Msg, int page = 1, int row =10)
        {
            if (Msg == "Success")
                ViewBag.Alert = "عضویت با موفقیت انجام شد";

            var pagingModel = PagingList.Create(await userManager.GetAllUsersWithRolesAsync(), row, page);
            return View(pagingModel);
        }
    }
}