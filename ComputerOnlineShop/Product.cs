using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class Product
    {
        public Product()
        {
            ClientProducts = new HashSet<ClientProduct>();
            ProductsInOrders = new HashSet<ProductsInOrder>();
            Properties = new HashSet<Property>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductDesc { get; set; }
        public byte[]? ImagePath { get; set; }
        public int SubcategoryId { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public decimal? AdditionalBonusCount { get; set; }

        public virtual Subcategory Subcategory { get; set; } = null!;
        public virtual ICollection<ClientProduct> ClientProducts { get; set; }
        public virtual ICollection<ProductsInOrder> ProductsInOrders { get; set; }
        public virtual ICollection<Property> Properties { get; set; }
    }
}
