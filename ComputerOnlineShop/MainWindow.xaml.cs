using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using BCrypt.Net;

namespace ComputerOnlineShop
{

    internal class GridProduct
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public decimal? AdditionalBonusCount { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DbContextOptions<computer_online_shopContext> dbOptions = computer_online_shopContext.GetOptions();
        public static Operator? LoggedUser;
        public PointsOfIssueWindow? PointsOfIssueWindow;
        public ClientOrdersWindow? ClientOrdersWindow;
        public UserInfoWindow? UserInfoWindow;

        public MainWindow()
        {
            Logic(); 
        }

        private async void Logic()
        {
            await PreAuth();
            InitializeComponent();
            await fillSubcategories();
        }

        private async Task PreAuth()
        {
            PasswordWindow passwordWindow = new();

            if (passwordWindow.ShowDialog() == true)
            {
                if (!(await authValidate(passwordWindow.UserName, passwordWindow.UserPassword)))
                {
                    await PreAuth();
                } else
                {
                    Show();
                }
            } else
            {
                Close();
            }

        }

        private async Task<bool> authValidate(string userName, string userPassword)
        {
            if (userName.Length == 0)
            {
                MessageBox.Show("Введите логин");
                return false;
            }
            if (userPassword.Length == 0)
            {
                MessageBox.Show("Введите пароль");
                return false;
            }

            using (computer_online_shopContext db = new(dbOptions))
            {
                Person? person = await (from Person in db.People
                                        where Person.UserName == userName
                                        select Person).FirstOrDefaultAsync();
                if (person == null)
                {
                    MessageBox.Show("Пользователь не найден");
                    return false;
                }

                bool checkPassword = BCrypt.Net.BCrypt.Verify(userPassword.Trim(), person.UserPassword);

                if (!checkPassword)
                {
                    MessageBox.Show("Неверный пароль");
                    return false;
                }

                Operator? @operator = await (from Operator in db.Operators
                                             where Operator.PersonId == person.Id
                                             select Operator).FirstOrDefaultAsync();
                if (@operator == null)
                {
                    MessageBox.Show("Нет прав для входа");
                    return false;
                }

                LoggedUser = @operator;
                /*MessageBox.Show("Пользователь авторизован");*/
                return true;
            }
        }

        private async void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            LoggedUser = null;
            Hide();
            if (PointsOfIssueWindow != null)
            {
                PointsOfIssueWindow.Close();
            }
            if (ClientOrdersWindow != null)
            {
                ClientOrdersWindow.Close();
            }
            if (UserInfoWindow != null)
            {
                UserInfoWindow.Close();
            }
            /*MessageBox.Show("Вы вышли из системы");*/
            await PreAuth();
        }

        private async Task fillSubcategories()
        {
            using (computer_online_shopContext db = new(dbOptions))
            {
                var subcategoryNames = await (from Subcategory in db.Subcategories select Subcategory.SubcategoryName).ToListAsync();
                subcategoriesList.ItemsSource = subcategoryNames;
                subcategoriesList.SelectedValue = subcategoryNames[0];
            }
        }

        private async void btnFillGrid_Click(object sender, RoutedEventArgs e)
        {
            using (computer_online_shopContext db = new(dbOptions))
            {
                var products = await (from Product in db.Products
                               join Subcategory in db.Subcategories on Product.SubcategoryId equals Subcategory.Id
                               where Subcategory.SubcategoryName == (string)subcategoriesList.SelectedValue
                               select new GridProduct
                               {
                                   Name = Product.ProductName,
                                   Description = Product.ProductDesc ?? "",
                                   Price = Product.Price,
                                   QuantityInStock = Product.QuantityInStock,
                                   AdditionalBonusCount = Product.AdditionalBonusCount,
                               }).ToListAsync();
                grid.ItemsSource = products;
            }
        }

        private void btnPointsOfIssue_Click(object sender, RoutedEventArgs e)
        {
            if (PointsOfIssueWindow == null)
            {
                PointsOfIssueWindow = new();
                PointsOfIssueWindow.Owner = this;
                PointsOfIssueWindow.Show();
            } else
            {
                PointsOfIssueWindow.Focus();
            }
        }

        private void btnClientOrders_Click(object sender, RoutedEventArgs e)
        {
            if (ClientOrdersWindow == null)
            {
                ClientOrdersWindow = new();
                ClientOrdersWindow.Owner = this;
                ClientOrdersWindow.Show();
            }
            else
            {
                ClientOrdersWindow.Focus();
            }
        }

        private async void showProperties_Click(object sender, RoutedEventArgs e)
        {
            string productName = propsProductName.Text;
            if (productName == "")
            {
                MessageBox.Show("Введите название товара");
                return;
            }
            using (computer_online_shopContext db = new(dbOptions))
            {
                var properties = await (from Property in db.Properties
                                        join Product in db.Products on Property.ProductId equals Product.Id 
                                        where Product.ProductName == productName
                                        select new GridProperty
                                        {
                                            PropertyType = Property.PropertyType.PropertyName,
                                            PropertyValue = Property.PropertyValue,
                                            MeasurementUnit = Property.PropertyType.MeasurementUnit.MeasurementUnitName,
                                            DataType = Property.PropertyType.DataType.DataTypeName
                                        }).ToListAsync();
                gridProperties.ItemsSource = properties;
            }
        }

        private void btnUserInfo_Click(object sender, RoutedEventArgs e)
        {
            if (UserInfoWindow == null)
            {
                UserInfoWindow = new();
                UserInfoWindow.Owner = this;
                UserInfoWindow.Show();
            }
            else
            {
                UserInfoWindow.Focus();
            }
        }
    }

    internal class InfoPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    internal class LoggedUser
    {
        public int Id { get; set; }
        public InfoPerson InfoPerson { get; set; }
    }

    internal class GridProperty
    {
        public string PropertyType { get; set; }
        public string PropertyValue { get; set; }
        public string MeasurementUnit { get; set; }
        public string DataType { get; set; }
    }
}
