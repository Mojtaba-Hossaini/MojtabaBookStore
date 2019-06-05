using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models.ViewModels.AccountViewModel
{
    public class ForgetPasswordViewModel
    {
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده نامعتبر است.")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Email { get; set; }
    }
}
