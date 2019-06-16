using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models.ViewModels.AccountViewModel
{
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [Display(Name = "کد اعتبارسنجی")]
        public string Code { get; set; }

        [Display(Name = "مرا به خاطر بسپار؟")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
