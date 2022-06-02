using System;
using System.Collections.Generic;

namespace ComputerOnlineShop
{
    public partial class Order
    {
        public Order()
        {
            ProductsInOrders = new HashSet<ProductsInOrder>();
            Refunds = new HashSet<Refund>();
        }

        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public string? ContactName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public int ClientId { get; set; }
        public int WayToReceiveId { get; set; }
        public int PaymentMethodId { get; set; }
        public string DeliveryAddress { get; set; } = null!;
        public int PointOfIssueId { get; set; }
        public DateOnly RegDate { get; set; }
        public DateOnly DateOfReceipt { get; set; }
        public DateOnly ActualDateOfReceipt { get; set; }
        public int OrderStatusId { get; set; }
        public decimal Price { get; set; }
        public DateTime RegTime { get; set; }
        public DateTime ReceiptTime { get; set; }
        public DateTime? ActualReceiptTime { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual OrderStatus OrderStatus { get; set; } = null!;
        public virtual PaymentMethod PaymentMethod { get; set; } = null!;
        public virtual PointsOfIssue PointOfIssue { get; set; } = null!;
        public virtual WaysToReceive WayToReceive { get; set; } = null!;
        public virtual ICollection<ProductsInOrder> ProductsInOrders { get; set; }
        public virtual ICollection<Refund> Refunds { get; set; }
    }
}
