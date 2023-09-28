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
    /// Logika interakcji dla klasy TransfersPage.xaml
    /// </summary>
    public partial class TransfersPage : Window
    {
        Client client = null;
        public TransfersPage(Client _client)
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
            displayCardData();
        }

        private void displayCardData()
        {
            //CARD COLOR
            if (client.cardColor == "Black")
            {
                checkingAccountCard_Rectangle.Fill = Brushes.Black;
            }
            else if (client.cardColor == "Brown")
            {
                checkingAccountCard_Rectangle.Fill = new SolidColorBrush(Color.FromRgb(64, 50, 40));
            }
            else
            {
                checkingAccountCard_Rectangle.Fill = new SolidColorBrush(Color.FromRgb(0, 52, 1));
            }

            string currency = " PLN";
            //CHECKING ACCOUNT
            if (client.cardActivity == 1)
            {
                valueOfCheckingAccount_Label.Content = client.checkingAccount + currency;
            }
            else
            {
                valueOfCheckingAccount_Label.Content = "-" + currency;
            }

            string cardNumber = client.cardNumber;
            if (cardNumber.Length == 16)
            {
                string formattedCardNumber = $"{cardNumber.Substring(0, 4)}  {cardNumber.Substring(4, 4)}  {cardNumber.Substring(8, 4)}  {cardNumber.Substring(12, 4)}";
                cardNumber_Label.Content = formattedCardNumber;
            }
            else
            {
                cardNumber_Label.Content = "XXXX  XXXX  XXXX  XXXX";
            }

            string cardEndDate = client.cardEndDate;
            if (client.cardActivity == 1 && cardEndDate.Length == 10)
            {
                string firstSevenCharacters = cardEndDate.Substring(0, 7);
                cardEndDate_Label.Content = firstSevenCharacters;
            }
            if (client.cardActivity == 0)
            {
                cardEndDate_Label.Content = "YYYY-MM";
            }

            string fullName = client.name + " " + client.surname;
            nameAndSurname_Label.Content = fullName;
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
            InvalidateVisual();
        }

        private void goToServicesPage(object sender, RoutedEventArgs e)
        {
            ServicesPage servicesPage = new ServicesPage(client);
            servicesPage.Show();
            this.Hide();
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

        private void goToDepositPage(object sender, RoutedEventArgs e)
        {
            if (isAccountActive(client.activity) && isCardActive(client.cardActivity))
            {
                TransfersDeposit deposit = new TransfersDeposit(client);
                deposit.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("You cannot make a transfer. Your account or card are inactive.", "eBank");
                return;
            }
        }

        private void goToWithdrawPage(object sender, RoutedEventArgs e)
        {
            if (isAccountActive(client.activity) && isCardActive(client.cardActivity))
            {
                TransfersWithdraw withdraw = new TransfersWithdraw(client);
                withdraw.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("You cannot make a transfer. Your account or card are inactive.", "eBank");
                return;
            }
        }

        private void goToRegularTransferPage(object sender, RoutedEventArgs e)
        {
            if (isAccountActive(client.activity) && isCardActive(client.cardActivity))
            {
                TransfersRegularTransfer regularTransfer = new TransfersRegularTransfer(client);
                regularTransfer.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("You cannot make a transfer. Your account or card are inactive.", "eBank");
                return;
            }
        }

        private void goToOwnTransferPage(object sender, RoutedEventArgs e)
        {
            if (isAccountActive(client.activity) && isCardActive(client.cardActivity))
            {
                TransfersOwnTransferFromCheckingAccount ownTransferFromCheckingAccount = new TransfersOwnTransferFromCheckingAccount(client);
                ownTransferFromCheckingAccount.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("You cannot make a transfer. Your account or card are inactive.", "eBank");
                return;
            }
        }

        private void goToBLIKPage(object sender, RoutedEventArgs e)
        {
            if (isAccountActive(client.activity) && isCardActive(client.cardActivity))
            {
                TransfersBLIK BLIK = new TransfersBLIK(client);
                BLIK.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("You cannot make a transfer. Your account or card are inactive.", "eBank");
                return;
            }
        }

        private void goToPhoneTopUpPage(object sender, RoutedEventArgs e)
        {
            if (isAccountActive(client.activity) && isCardActive(client.cardActivity))
            {
                TransfersPhoneTopUp phoneTopUp = new TransfersPhoneTopUp(client);
                phoneTopUp.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("You cannot make a transfer. Your account or card are inactive.", "eBank");
                return;
            }
        }

        private void goToCurrencyTransferPage(object sender, RoutedEventArgs e)
        {
            if (isAccountActive(client.activity) && isCardActive(client.cardActivity))
            {
                TransfersCurrencyTransfer currencyTransfer = new TransfersCurrencyTransfer(client);
                currencyTransfer.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("You cannot make a transfer. Your account or card are inactive.", "eBank");
                return;
            }
        }

        private void goToTaxTransferPage(object sender, RoutedEventArgs e)
        {
            if (isAccountActive(client.activity) && isCardActive(client.cardActivity))
            {
                TransfersTaxTransfer taxTransfer = new TransfersTaxTransfer(client);
                taxTransfer.Show();
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
