using System;
using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models.ViewModels
{
    public class BooksIndexViewModel
    {
        public int BookID { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "قیمت")]
        public int Price { get; set; }

        [Display(Name = "موجودی")]
        public int Stock { get; set; }

        [Display(Name = "شابک")]
        public string ISBN { get; set; }

        [Display(Name = "ناشر")]
        public string PublisherName { get; set; }

        [Display(Name = "تاریخ انتشار در سایت")]
        public DateTime? PublishDate { get; set; }

        [Display(Name = "وضعیت")]
        public bool? IsPublish { get; set; }

        [Display(Name = "نویسندگان")]
        public string Author { get; set; }

        public string Translator { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
    }
}
