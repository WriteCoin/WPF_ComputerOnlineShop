using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class Client
    {
        public Client()
        {
            ClientProducts = new HashSet<ClientProduct>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int PersonId { get; set; }
        public decimal? Balance { get; set; }
        public decimal? BonusCount { get; set; }

        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<ClientProduct> ClientProducts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
