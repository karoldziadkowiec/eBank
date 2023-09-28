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
            displayAdminPanel();
            displayData();
        }

        private void displayAdminPanel()
        {
            if (client.accountType == "Admin")
            {
                adminPanel_Rectangle.Visibility = Visibility.Visible;
                clients_Button.Visibility = Visibility.Visible;
                clients_Image.Visibility = Visibility.Visible;
                sep1_Rectangle.Visibility = Visibility.Visible;
                transactions_Button.Visibility = Visibility.Visible;
                transactions_Image.Visibility = Visibility.Visible;
                sep2_Rectangle.Visibility = Visibility.Visible;
                permissions_Button.Visibility = Visibility.Visible;
                permissions_Image.Visibility = Visibility.Visible;
            }
            else
            {
                adminPanel_Rectangle.Visibility = Visibility.Hidden;
                clients_Button.Visibility = Visibility.Hidden;
                clients_Image.Visibility = Visibility.Hidden;
                sep1_Rectangle.Visibility = Visibility.Hidden;
                transactions_Button.Visibility = Visibility.Hidden;
                transactions_Image.Visibility = Visibility.Hidden;
                sep2_Rectangle.Visibility = Visibility.Hidden;
                permissions_Button.Visibility = Visibility.Hidden;
                permissions_Image.Visibility = Visibility.Hidden;
            }
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

        private void goToClientsPage(object sender, RoutedEventArgs e)
        {
            AdminClientsPage clientsPage = new AdminClientsPage(client);
            clientsPage.Show();
            this.Hide();
        }

        private void goToTransactionsPage(object sender, RoutedEventArgs e)
        {
            AdminTransactionsPage transactionsPage = new AdminTransactionsPage(client);
            transactionsPage.Show();
            this.Hide();
        }

        private void goToPermissionsPage(object sender, RoutedEventArgs e)
        {
            AdminPermissionsPage permissionsPage = new AdminPermissionsPage(client);
            permissionsPage.Show();
            this.Hide();
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
