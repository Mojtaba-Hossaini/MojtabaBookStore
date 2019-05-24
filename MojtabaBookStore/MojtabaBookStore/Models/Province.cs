using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MojtabaBookStore.Models
{
    public class Province
    {
        [Key]
        public int ProvinceID { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string ProvinceName { get; set; }

        public List<City> Cities { get; set; }
    }
}
