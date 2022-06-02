using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string PaymentMethodName { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
