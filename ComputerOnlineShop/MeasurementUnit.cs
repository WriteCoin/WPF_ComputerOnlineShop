using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class MeasurementUnit
    {
        public MeasurementUnit()
        {
            PropertyTypes = new HashSet<PropertyType>();
        }

        public int Id { get; set; }
        public string MeasurementUnitName { get; set; } = null!;

        public virtual ICollection<PropertyType> PropertyTypes { get; set; }
    }
}
