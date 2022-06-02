using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class Operator
    {
        public int Id { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; } = null!;
    }
}
