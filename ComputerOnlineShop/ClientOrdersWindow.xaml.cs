using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;

namespace ComputerOnlineShop
{
    internal class GridOrder
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string WayToReceive { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderStatus { get; set; }
        public string PointOfIssue { get; set; }
        public string RegDate { get; set; }
        public string DateOfReceipt { get; set; }
        public string ActualDateOfReceipt { get; set; }
        public string Price { get; set; }
        public string RegTime { get; set; }
        public string ReceiptTime { get; set; }
        public string ActualReceiptTime { get; set; }
    }
    /// <summary>
    /// Логика взаимодействия для ClientOrdersWindow.xaml
    /// </summary>
    public partial class ClientOrdersWindow : Window
    {
        public ClientOrdersWindow()
        {
            InitializeComponent();
            fillData();
        }

        private async void fillData()
        {
            using (computer_online_shopContext db = new(MainWindow.dbOptions))
            {
                var wayToReceives = await (from WaysToReceive in db.WaysToReceives select WaysToReceive.WayToReceiveName).ToListAsync();
                var paymentMethods = await (from PaymentMethod in db.PaymentMethods select PaymentMethod.PaymentMethodName).ToListAsync();
                var orderStatuses = await (from OrderStatus in db.OrderStatuses select OrderStatus.OrderStatusName).ToListAsync();
                wayToReceive.ItemsSource = wayToReceives;
                wayToReceive.SelectedValue = wayToReceives[0];
                paymentMethod.ItemsSource = paymentMethods;
                paymentMethod.SelectedValue = paymentMethods[0];
                orderStatus.ItemsSource = orderStatuses;
                orderStatus.SelectedValue = orderStatuses[0];
            }
        }

        private async void btnGetOrders_Click(object sender, RoutedEventArgs e)
        {
            using (computer_online_shopContext db = new(MainWindow.dbOptions))
            {
                try
                {
                    int OrderNumber = orderNumber.Text == "" ? 0 : int.Parse(orderNumber.Text);
                    var orders = await (from Order in db.Orders
                                        join WaysToReceive in db.WaysToReceives on Order.WayToReceiveId equals WaysToReceive.Id
                                        join PaymentMethod in db.PaymentMethods on Order.PaymentMethodId equals PaymentMethod.Id
                                        join PointsOfIssue in db.PointsOfIssues on Order.PointOfIssueId equals PointsOfIssue.Id
                                        join OrderStatus in db.OrderStatuses on Order.OrderStatusId equals OrderStatus.Id
                                        join Client in db.Clients on Order.ClientId equals Client.Id
                                        where WaysToReceive.WayToReceiveName == wayToReceive.SelectedValue &&
                                              PaymentMethod.PaymentMethodName == paymentMethod.SelectedValue &&
                                              OrderStatus.OrderStatusName == orderStatus.SelectedValue &&
                                              (OrderNumber == 0 || Order.OrderNumber == OrderNumber)
                                        select new GridOrder
                                        {
                                            Id = Order.Id,
                                            OrderNumber = Order.OrderNumber.ToString(),
                                            ContactName = Order.ContactName,
                                            ContactEmail = Order.ContactEmail,
                                            ContactPhone = Order.ContactPhone,
                                            WayToReceive = WaysToReceive.WayToReceiveName,
                                            PaymentMethod = PaymentMethod.PaymentMethodName,
                                            OrderStatus = OrderStatus.OrderStatusName,
                                            PointOfIssue = PointsOfIssue.Name,
                                            RegDate = Order.RegDate.ToShortDateString(),
                                            DateOfReceipt = Order.DateOfReceipt.ToShortDateString(),
                                            ActualDateOfReceipt = Order.ActualDateOfReceipt.ToShortDateString(),
                                            Price = Order.Price.ToString(),
                                            RegTime = Order.RegTime.ToShortTimeString(),
                                            ReceiptTime = Order.ReceiptTime.ToShortTimeString(),
                                            ActualReceiptTime = Order.ActualReceiptTime.ToString(),
                                        }).ToListAsync();
                    grid.ItemsSource = orders;

                } catch (Exception ex)
                {
                    MessageBox.Show("Ошибка");
                }
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool isError = false;
            using (computer_online_shopContext db = new(MainWindow.dbOptions))
            {
                try
                {
                    foreach (var Item in (dynamic)grid.ItemsSource)
                    {
                        if (isError)
                        {
                            break;
                        }
                        // string format = "HH:mm";
                        string orderStatus = Item.OrderStatus;
                        var StatusOrder = await (from OrderStatus in db.OrderStatuses where OrderStatus.OrderStatusName == orderStatus select OrderStatus.Id).ToListAsync();
                        int OrderStatusId = StatusOrder[0];
                        Order data = new Order { Id = Item.Id, DateOfReceipt = DateOnly.Parse(Item.DateOfReceipt), ActualDateOfReceipt = DateOnly.Parse(Item.ActualDateOfReceipt), ReceiptTime = DateTime.Parse(Item.ReceiptTime), ActualReceiptTime = DateTime.Parse(Item.ActualReceiptTime), OrderStatusId = OrderStatusId };
                        // MessageBox.Show(data.Id.ToString());
                        Order order = await db.Orders.FirstOrDefaultAsync(o => o.Id == data.Id);
                        order.DateOfReceipt = data.DateOfReceipt;
                        order.ActualDateOfReceipt = data.ActualDateOfReceipt;
                        order.ReceiptTime = data.ReceiptTime;
                        order.ActualReceiptTime = data.ActualReceiptTime;
                        order.OrderStatusId = data.OrderStatusId;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Ошибка {ex.Message} {ex.StackTrace}");
                    // MessageBox.Show("Ошибка");
                    isError = true;
                }
                if (!isError)
                {
                    await db.SaveChangesAsync();
                    MessageBox.Show("Данные изменены успешно");
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindow)this.Owner).ClientOrdersWindow = null;
        }
    }
}
