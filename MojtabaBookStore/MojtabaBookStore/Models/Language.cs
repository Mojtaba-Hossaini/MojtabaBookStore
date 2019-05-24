using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models
{
    public class Language
    {
        public int LanguageID { get; set; }

        [Display(Name = "زبان")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string LanguageName { get; set; }

        public List<Book> Books { get; set; }
    }
}
