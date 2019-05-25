using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models
{
    public class Publisher
    {
        public int PublisherID { get; set; }

        [Display(Name = "ناشر")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string PublisherName { get; set; }
        public List<Book> Books { get; set; }
    }
}
