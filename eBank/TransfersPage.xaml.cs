﻿using System;
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

            }
            else
            {
                MessageBox.Show("You cannot make a transfer. Your account or card are inactive.", "eBank");
                return;
            }
        }
    }
}
