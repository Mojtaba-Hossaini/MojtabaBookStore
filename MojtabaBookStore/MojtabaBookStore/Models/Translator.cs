using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models
{
    public class Translator
    {
        public int TranslatorID { get; set; }

        [Display(Name="نام")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی میباشد")]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "وارد کردن {0} الزامی میباشد")]
        public string Family { get; set; }
        public List<Book_Translator> Book_Translators { get; set; }
    }
}
