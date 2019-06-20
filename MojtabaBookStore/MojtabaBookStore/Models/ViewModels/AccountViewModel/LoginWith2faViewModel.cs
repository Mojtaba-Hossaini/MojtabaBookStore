using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models.ViewModels.AccountViewModel
{
    public class LoginWith2faViewModel
    {
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [StringLength(7, ErrorMessage = "کد اعتبارسنجی با حداقل دارای {2} کاراکتر و حداکثر دارای {1} کاراکتر باشد.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "کد اعتبارسنجی")]
        public string TwoFactorCode { get; set; }

        public bool RememberMe { get; set; }

        [Display(Name = "مرا به خاطر بسپار؟")]
        public bool RememberMachine { get; set; }
    }
}
