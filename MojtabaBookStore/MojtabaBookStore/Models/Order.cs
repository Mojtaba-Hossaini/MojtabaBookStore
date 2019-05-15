using System;
using System.Collections.Generic;

namespace MojtabaBookStore.Models
{
    public class Order
    {
        public string OrderID { get; set; }
        public long AmountPaid { get; set; }
        public string DispatchNumber { get; set; }
        public DateTime BuyDate { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public Customer Customer { get; set; }
        public List<Order_Book> Order_Books { get; set; }
    }
}
