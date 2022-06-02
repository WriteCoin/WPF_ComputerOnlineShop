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
    internal class GridPointOfIssue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string WorkTimeStart { get; set; }
        public string WorkTimeEnd { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class PointsOfIssueWindow : Window
    {
        MainWindow owner;
        public PointsOfIssueWindow()
        {
            InitializeComponent();
            owner = (MainWindow)this.owner;
            fillData();
        }

        private async void fillData()
        {
            using (computer_online_shopContext db = new(MainWindow.dbOptions))
            {
                var data = await (from PointsOfIssue in db.PointsOfIssues select new GridPointOfIssue
                {
                    Id = PointsOfIssue.Id,
                    Name = PointsOfIssue.Name,
                    Address = PointsOfIssue.Address,
                    WorkTimeStart = PointsOfIssue.WorkTimeStart.ToShortTimeString(),
                    WorkTimeEnd = PointsOfIssue.WorkTimeEnd.ToShortTimeString()
                }).ToListAsync();
                grid.ItemsSource = data;
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool isError = false;
            using (computer_online_shopContext db = new(MainWindow.dbOptions))
            {
                try
                {
                    foreach (var Item in (dynamic) grid.ItemsSource)
                    {
                        if (isError)
                        {
                            break;
                        }
                        /*MessageBox.Show(Item.Id);
                        MessageBox.Show(Item.WorkTimeStart);*/
/*                        WorkTimeStart = TimeOnly.Parse(Item.WorkTimeStart), WorkTimeEnd = TimeOnly.Parse(Item.WorkTimeEnd)
*/                        PointsOfIssue data = new PointsOfIssue { Id = Item.Id, Name = Item.Name, Address = Item.Address, WorkTimeStart = TimeOnly.Parse(Item.WorkTimeStart), WorkTimeEnd = TimeOnly.Parse(Item.WorkTimeEnd) };
                        PointsOfIssue point = await db.PointsOfIssues.FirstOrDefaultAsync(p => p.Id == data.Id);
                        if (point != null)
                        {
                            point.Id = data.Id;
                            point.Name = data.Name;
                            point.Address = data.Address;
                            point.WorkTimeStart = data.WorkTimeStart;
                            point.WorkTimeEnd = data.WorkTimeEnd;
                        }
                    } 
                }
                catch (Exception ex)
                {
                    // MessageBox.Show($"Ошибка {ex.Message} {ex.StackTrace}");
                    MessageBox.Show("Ошибка");
                    isError = true;
                }
                if (!isError)
                {
                    await db.SaveChangesAsync();
                    MessageBox.Show("Данные изменены успешно");
                }
            }
        }

        private async void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            bool isError = false;
            using (computer_online_shopContext db = new (MainWindow.dbOptions)){
                try
                {
                    foreach (var Item in (dynamic)gridAdd.ItemsSource)
                    {
                        PointsOfIssue data = new PointsOfIssue { Name = Item.Name, Address = Item.Address, WorkTimeStart = TimeOnly.Parse(Item.WorkTimeStart), WorkTimeEnd = TimeOnly.Parse(Item.WorkTimeEnd) };
                        db.PointsOfIssues.Add(data);
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка");
                    isError = true;
                }
                if (!isError)
                {
                    await db.SaveChangesAsync();
                    MessageBox.Show("Данные добавлены успешно");
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindow)this.Owner).PointsOfIssueWindow = null;
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            bool isError = false;
            using (computer_online_shopContext db = new(MainWindow.dbOptions))
            {
                try
                {
                    foreach (var Item in (dynamic)gridAdd.ItemsSource)
                    {
                        PointsOfIssue data = new PointsOfIssue { Name = Item.Name, Address = Item.Address, WorkTimeStart = TimeOnly.Parse(Item.WorkTimeStart), WorkTimeEnd = TimeOnly.Parse(Item.WorkTimeEnd) };
                        PointsOfIssue point = await db.PointsOfIssues.FirstOrDefaultAsync(p => p.Name == data.Name && p.Address == data.Address && p.WorkTimeStart == data.WorkTimeStart && p.WorkTimeEnd == data.WorkTimeEnd);
                        db.PointsOfIssues.Remove(point);
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка: такой пункт доставки не найден");
                    isError = true;
                }
                if (!isError)
                {
                    await db.SaveChangesAsync();
                    MessageBox.Show("Данные удалены успешно");
                }
            }
        }
    }
}
