using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models.ViewModels.UsersManager
{
    public class UserResetPasswordViewModel
    {
        public string Id { get; set; }

        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور جدید")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string NewPassword { get; set; }
    }
}
