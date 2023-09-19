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

namespace eBank
{
    /// <summary>
    /// Logika interakcji dla klasy AdminHomePage.xaml
    /// </summary>
    public partial class AdminHomePage : Window
    {
        Client client = null;
        public AdminHomePage(Client _client)
        {
            client = _client;
            InitializeComponent();
            date_Label.Content = DateTime.Now.ToString("dd.MM.yyyy");
        }
        private void goToHomePage(object sender, RoutedEventArgs e)
        {
            InvalidateVisual();
        }

        private void goToHistoryPage(object sender, RoutedEventArgs e)
        {
            
        }

        private void goToTransfersPage(object sender, RoutedEventArgs e)
        {
            
        }

        private void goToServicesPage(object sender, RoutedEventArgs e)
        {
            
        }

        private void goToSettingsPage(object sender, RoutedEventArgs e)
        {
            
        }

        private void goToAccountPage(object sender, RoutedEventArgs e)
        {
            
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "eBank", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MainWindow loginPage = new MainWindow();
                loginPage.Show();
                this.Hide();
            }
        }
    }
}
