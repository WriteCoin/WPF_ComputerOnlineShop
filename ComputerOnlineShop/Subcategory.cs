using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class Subcategory
    {
        public Subcategory()
        {
            Products = new HashSet<Product>();
            PropertyTypes = new HashSet<PropertyType>();
        }

        public int Id { get; set; }
        public string SubcategoryName { get; set; } = null!;
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<PropertyType> PropertyTypes { get; set; }
    }
}
