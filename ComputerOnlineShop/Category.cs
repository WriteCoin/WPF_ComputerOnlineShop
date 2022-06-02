using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class Category
    {
        public Category()
        {
            Subcategories = new HashSet<Subcategory>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Subcategory> Subcategories { get; set; }
    }
}
