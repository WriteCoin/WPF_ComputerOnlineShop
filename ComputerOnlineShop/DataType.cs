using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class DataType
    {
        public DataType()
        {
            PropertyTypes = new HashSet<PropertyType>();
        }

        public int Id { get; set; }
        public string DataTypeName { get; set; } = null!;

        public virtual ICollection<PropertyType> PropertyTypes { get; set; }
    }
}
