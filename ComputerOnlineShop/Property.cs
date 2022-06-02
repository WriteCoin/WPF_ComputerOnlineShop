using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class Property
    {
        public int Id { get; set; }
        public int PropertyTypeId { get; set; }
        public int ProductId { get; set; }
        public string PropertyValue { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
        public virtual PropertyType PropertyType { get; set; } = null!;
    }
}
