using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class PropertyType
    {
        public PropertyType()
        {
            Properties = new HashSet<Property>();
        }

        public int Id { get; set; }
        public string PropertyName { get; set; } = null!;
        public int MeasurementUnitId { get; set; }
        public int DataTypeId { get; set; }
        public int SubcategoryId { get; set; }

        public virtual DataType DataType { get; set; } = null!;
        public virtual MeasurementUnit MeasurementUnit { get; set; } = null!;
        public virtual Subcategory Subcategory { get; set; } = null!;
        public virtual ICollection<Property> Properties { get; set; }
    }
}
