using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using MojtabaBookStore.Areas.Identity.Data;
using MojtabaBookStore.Models.ViewModels;
using ReflectionIT.Mvc.Paging;

namespace MojtabaBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesManagerController : Controller
    {
        private readonly IApplicationRoleManager roleManger;

        public RolesManagerController(IApplicationRoleManager roleManger)
        {
            this.roleManger = roleManger;
        }
        public IActionResult Index(int page=1, int row = 5)
        {
            var roles = roleManger.GetAllRolesAndUsersCount();
            var pagingModel = PagingList.Create(roles, row, page);
            pagingModel.RouteValue = new RouteValueDictionary { { "row", row } };
            return View(pagingModel);
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(RolesViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            
            var result = await roleManger.CreateAsync(new ApplicationRole(viewModel.RoleName, viewModel.Description));
            if (result.Succeeded)
                return RedirectToAction("Index");
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            if (id == null)
                return NotFound();

            var role = await roleManger.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            var roleVm = new RolesViewModel { RoleID = role.Id, RoleName = role.Name , Description = role.Description};
            return View(roleVm);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(RolesViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var role = await roleManger.FindByIdAsync(viewModel.RoleID);
            if (role == null)
                return NotFound();

            
            role.Name = viewModel.RoleName;
            role.Description = viewModel.Description;
            var result = await roleManger.UpdateAsync(role);

            if (result.Succeeded)
            {
                ViewBag.Success = "ذخیره تغییرات با موقیت انجام شد";
                //return View(viewModel);
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

           
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (id == null)
                return NotFound();

            var role = await roleManger.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            var viewModel = new RolesViewModel { RoleID = role.Id, RoleName = role.Name };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteRole")]
        public async Task<IActionResult> DeletedRole(string id)
        {
            if (id == null)
                return NotFound();

            var role = await roleManger.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            var result = await roleManger.DeleteAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");

            ViewBag.Error = "در حذف اطلاعات خطایی رخ داده است";

            var viewModel = new RolesViewModel { RoleID = role.Id, RoleName = role.Name };
            return View(viewModel);
        }
    }
}