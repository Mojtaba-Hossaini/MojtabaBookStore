using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models.ViewModels.AccountViewModel
{
    public class LoginWithRecoveryCodeViewModel
    {
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        [DataType(DataType.Text)]
        [Display(Name = "کد بازیابی")]
        public string RecoveryCode { get; set; }
    }
}
