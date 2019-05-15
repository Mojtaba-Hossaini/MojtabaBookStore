using System.Collections.Generic;

namespace MojtabaBookStore.Models
{
    public class OrderStatus
    {
        public int OrderStatusID { get; set; }
        public string OrderStatusName { get; set; }

        public List<Order> Orders { get; set; }
    }
}
