using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MojtabaBookStore.Models;

namespace MojtabaBookStore.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index(string id)
        {
            if(id != null)
                ViewBag.ConfirmEmailAlert = "لینک فعال سازی حساب کاربری به ایمیل شما ارسال شد لطفا با کلیک روی این لینک حساب خود را فعال کنید.";
            return View();
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
