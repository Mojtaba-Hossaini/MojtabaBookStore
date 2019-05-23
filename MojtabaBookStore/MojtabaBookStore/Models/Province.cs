using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MojtabaBookStore.Models
{
    public class Province
    {
        [Key]
        public int ProvinceID { get; set; }

        [Display(Name = "نام استان")]
        public string ProvinceName { get; set; }

        public List<City> Cities { get; set; }
    }
}
