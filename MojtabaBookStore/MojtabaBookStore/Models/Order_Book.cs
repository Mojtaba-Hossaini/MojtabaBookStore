using System.ComponentModel.DataAnnotations;

namespace MojtabaBookStore.Models
{
    public class Order_Book
    {
        public string OrderID { get; set; }
        public int BookID { get; set; }

        public Order Order { get; set; }
        public Book Book { get; set; }
    }
}
