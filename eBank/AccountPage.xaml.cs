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
    /// Logika interakcji dla klasy AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Window
    {
        Client client = null;
        public AccountPage(Client _client)
        {
            client = _client;
            InitializeComponent();
            displayData();
        }

        private void displayData()
        {
            date_Label.Content = DateTime.Now.ToString("yyyy-MM-dd");
            string currency = " PLN";
            valueOfCheckingAccount_Label.Content = client.checkingAccount.ToString() + currency;
            valueOfSavingsAccount_Label.Content = client.savingsAccount.ToString() + currency;
            double overallValue = client.checkingAccount + client.savingsAccount;
            overallValue_Label.Content = overallValue.ToString() + currency;
            if (client.cardNumber == "")
            {
                valueOfCardNumber_Label.Content = "-";
            }
            else
            {
                valueOfCardNumber_Label.Content = client.cardNumber;
            }

            if (client.cardActivity == 0)
            {
                valueOfCardStatus_Label.Content = "inactive";
                valueOfCardStatus_Label.Foreground = Brushes.Red;
            }
            else if (client.cardActivity == 1)
            {
                valueOfCardStatus_Label.Content = "active";
                valueOfCardStatus_Label.Foreground = Brushes.LightSeaGreen;
            }

            if (client.cardColor == "")
            {
                valueOfCardColor_Label.Content = "-";
            }
            else
            {
                valueOfCardColor_Label.Content = client.cardColor;
            }
            
            valueOfCardWithdrawalLimit_Label.Content = client.withdrawalLimit + currency;
            valueOfCardTransactionLimit_Label.Content = client.transactionLimit + currency;

            createCard_Button.Visibility = Visibility.Hidden;
            if (client.cardStartDate == "1900-01-01" || client.cardStartDate == "")
            {
                cardStartDate_Label.Visibility = Visibility.Hidden;
                valueOfCardStartDate_Label.Visibility = Visibility.Hidden;
                createCard_Button.Visibility = Visibility.Visible;
            }
            else
            {
                valueOfCardStartDate_Label.Content = client.cardStartDate;
            }

            if (client.cardEndDate == "1900-01-01" || client.cardEndDate == "")
            {
                cardEndDate_Label.Visibility = Visibility.Hidden;
                valueOfCardEndDate_Label.Visibility = Visibility.Hidden;
            }
            else
            {
                valueOfCardEndDate_Label.Content = client.cardEndDate;
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
            SettingsPage settingsPage = new SettingsPage(client);
            settingsPage.Show();
            this.Hide();
        }

        private void goToAccountPage(object sender, RoutedEventArgs e)
        {
            InvalidateVisual();
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
            ServicesOrderCard servicesOrderCard = new ServicesOrderCard(client);
            servicesOrderCard.Show();
            this.Hide();
        }
    }
}
