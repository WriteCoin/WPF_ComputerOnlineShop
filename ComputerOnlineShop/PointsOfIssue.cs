using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class PointsOfIssue
    {
        public PointsOfIssue()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public TimeOnly WorkTimeStart { get; set; }
        public TimeOnly WorkTimeEnd { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
