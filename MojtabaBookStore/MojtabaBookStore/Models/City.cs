using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MojtabaBookStore.Models
{
    public class City
    {
        [Key]
        public int CityID { get; set; }
        public string CityName { get; set; }

        public Province Provice { get; set; }

        public List<Customer> Customers1 { get; set; }

        public List<Customer> Customers2 { get; set; }
    }
}
