using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class WaysToReceive
    {
        public WaysToReceive()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string WayToReceiveName { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
