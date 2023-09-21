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
    /// Logika interakcji dla klasy ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Window
    {
        Client client = null;
        public ServicesPage(Client _client)
        {
            client = _client;
            InitializeComponent();
            displayData();
        }

        private void displayData()
        {
            date_Label.Content = DateTime.Now.ToString("yyyy-MM-dd");
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
            InvalidateVisual();
        }

        private void goToSettingsPage(object sender, RoutedEventArgs e)
        {
            SettingsPage settingsPage = new SettingsPage(client);
            settingsPage.Show();
            this.Hide();
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

        private void goToOrderCardPage(object sender, RoutedEventArgs e)
        {
            if (client.cardActivity == 0)
            {
                ServicesOrderCard servicesOrderCard = new ServicesOrderCard(client);
                servicesOrderCard.Show();
                this.Hide();
            }
            else if (client.cardActivity == 1) {
                MessageBox.Show("You already have an active card.", "eBank");
            }
        }

        private void goToExpenseAnalysisPage(object sender, RoutedEventArgs e)
        {

        }

        private void goToTransferRequestPage(object sender, RoutedEventArgs e)
        {

        }

        private void goToGamesAndGiftCardsPage(object sender, RoutedEventArgs e)
        {

        }

        private void goToPublicTransportTicketsPage(object sender, RoutedEventArgs e)
        {

        }

        private void goToHighwayTicketsPage(object sender, RoutedEventArgs e)
        {

        }

        private void goToParkingTicketsPage(object sender, RoutedEventArgs e)
        {

        }
    }
}
