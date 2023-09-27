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

        private bool isAccountActive(int accountStatus)
        {
            if (accountStatus == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isCardActive(int cardStatus)
        {
            if (cardStatus == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void goToOrderCardPage(object sender, RoutedEventArgs e)
        {
            if (client.cardActivity == 0)
            {
                ServicesOrderCard orderCard = new ServicesOrderCard(client);
                orderCard.Show();
                this.Hide();
            }
            else if (client.cardActivity == 1) {
                MessageBox.Show("You already have an active card.", "eBank");
            }
        }

        private void goToAccountAnalysisPage(object sender, RoutedEventArgs e)
        {
            ServicesAccountAnalysis accountAnalysis = new ServicesAccountAnalysis(client);
            accountAnalysis.Show();
            this.Hide();
        }

        private void goToTransferRequestPage(object sender, RoutedEventArgs e)
        {
            if (isAccountActive(client.activity) && isCardActive(client.cardActivity))
            {
                ServicesTransferRequest transferRequest = new ServicesTransferRequest(client);
                transferRequest.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("You cannot make a transfer. Your account or card are inactive.", "eBank");
                return;
            }
        }

        private void goToGameAndGiftCardsPage(object sender, RoutedEventArgs e)
        {
            if (isAccountActive(client.activity) && isCardActive(client.cardActivity))
            {
                ServicesGameAndGiftCards gameAndGiftCards = new ServicesGameAndGiftCards(client);
                gameAndGiftCards.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("You cannot make a transfer. Your account or card are inactive.", "eBank");
                return;
            }
        }

        private void goToPublicTransportTicketsPage(object sender, RoutedEventArgs e)
        {
            if (isAccountActive(client.activity) && isCardActive(client.cardActivity))
            {
                ServicesPublicTransportTickets publicTransportTickets = new ServicesPublicTransportTickets(client);
                publicTransportTickets.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("You cannot make a transfer. Your account or card are inactive.", "eBank");
                return;
            }
        }

        private void goToParkingTicketsPage(object sender, RoutedEventArgs e)
        {
            if (isAccountActive(client.activity) && isCardActive(client.cardActivity))
            {
                ServicesParkingTickets parkingTickets = new ServicesParkingTickets(client);
                parkingTickets.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("You cannot make a transfer. Your account or card are inactive.", "eBank");
                return;
            }
        }
    }
}
