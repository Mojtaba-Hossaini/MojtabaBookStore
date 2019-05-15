using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MojtabaBookStore.Models
{
    public class Discount
    {
        public int BookID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte Percent { get; set; }

        public Book Book { get; set; }
    }
}
