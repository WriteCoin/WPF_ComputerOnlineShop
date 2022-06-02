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
    /// <summary>
    /// Логика взаимодействия для UserInfoWindow.xaml
    /// </summary>
    public partial class UserInfoWindow : Window
    {
        public UserInfoWindow()
        {
            InitializeComponent();
            fillInfo();
        }

        private async void fillInfo()
        {
            using (computer_online_shopContext db = new(MainWindow.dbOptions))
            {
                Person user = MainWindow.LoggedUser.Person;
                FirstName.Text = user.FirstName;
                LastName.Text = user.LastName;
                UserName.Text = user.UserName;
                Phone.Text = user.Phone;
                Email.Text = user.Email;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainWindow)this.Owner).UserInfoWindow = null;
        }
    }
}
