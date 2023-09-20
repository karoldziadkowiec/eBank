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
    /// Logika interakcji dla klasy SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Window
    {
        Client client = null;
        public SettingsPage(Client _client)
        {
            client = _client;
            InitializeComponent();
            displayData();
        }

        private void displayData() {
            date_Label.Content = DateTime.Now.ToString("dd.MM.yyyy");
            withdrawalLimit_TextBox.Text = client.withdrawalLimit.ToString();
            transactionLimit_TextBox.Text = client.transactionLimit.ToString();

            if (client.activity == 1)
            {
                accountActivity_Label.Content = "active";
            }
            else
            {
                accountActivity_Label.Content = "inactive";
            }

            if (client.cardActivity == 1)
            {
                cardActivity_Label.Content = "active";
            }
            else
            {
                cardActivity_Label.Content = "inactive";
                block_Button.Visibility = Visibility.Hidden;
            }
        }

        private void goToHomePage(object sender, RoutedEventArgs e)
        {
            HomePage homePage = new HomePage(client);
            homePage.Show();
            this.Hide();
        }

        private void goToHistoryPage(object sender, RoutedEventArgs e)
        {
            HistoryPage historyPage = new HistoryPage(client);
            historyPage.Show();
            this.Hide();
        }

        private void goToTransfersPage(object sender, RoutedEventArgs e)
        {
            TransfersPage transfersPage = new TransfersPage(client);
            transfersPage.Show();
            this.Hide();
        }

        private void goToServicesPage(object sender, RoutedEventArgs e)
        {
            ServicesPage servicesPage = new ServicesPage(client);
            servicesPage.Show();
            this.Hide();
        }

        private void goToSettingsPage(object sender, RoutedEventArgs e)
        {
            InvalidateVisual();
        }

        private void goToAccountPage(object sender, RoutedEventArgs e)
        {
            AccountPage accountPage = new AccountPage(client);
            accountPage.Show();
            this.Hide();
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

        private void saveWithdrawalLimit(object sender, RoutedEventArgs e)
        {

        }

        private void saveTransactionLimit(object sender, RoutedEventArgs e)
        {

        }

        private void changeAccountActivity(object sender, RoutedEventArgs e)
        {

        }

        private void blockCard(object sender, RoutedEventArgs e)
        {

        }
    }
}
