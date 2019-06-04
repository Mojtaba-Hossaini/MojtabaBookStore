using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models.ViewModels.AccountViewModel
{
    public class SignInViewModel : GoogleRecaptchaModelBase
    {
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار؟")]
        public bool RememberMe { get; set; }

        //[Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        //[StringLength(4, ErrorMessage = "کد امنیتی باید دارای 4 کاراکتر باشد.")]
        //[Display(Name = "کد امنیتی")]
        //public string CaptchaCode { get; set; }
    }
}
