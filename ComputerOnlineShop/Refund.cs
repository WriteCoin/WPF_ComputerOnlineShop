using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class Refund
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateOnly DateOfRefund { get; set; }
        public string? ReasonForRefund { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
