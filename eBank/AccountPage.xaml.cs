using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client = null;
        public AccountPage(Client _client)
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
            displayAccountData();
            displayCardData();
        }

        private void displayAccountData()
        {
            string nameAndSurname = client.name + " " + client.surname;
            nameAndSurname_Label.Content = nameAndSurname;
            string accountCreated = "account created ";
            creationDate_Label.Content = accountCreated + client.creationDate.ToString();
            if (client.activity == 0)
            {
                valueOfAccountStatus_Label.Content = "inactive";
                valueOfAccountStatus_Label.Foreground = Brushes.Red;
            }
            else if (client.activity == 1)
            {
                valueOfAccountStatus_Label.Content = "active";
                valueOfAccountStatus_Label.Foreground = Brushes.Green;
            }
            valueOfAccountType_Label.Content = client.accountType;
            valueOfLogin_Label.Content = client.login;
            valueOfPeselNumber_Label.Content = client.peselNumber;
            valueOfIdCardNumber_Label.Content = client.idCardNumber;
            valueOfGender_Label.Content = client.gender;
            valueOfBirthday_Label.Content = client.birthday;
            valueOfTelephoneNumber_Label.Content = client.phoneNumber;
            valueOfEmail_Label.Content = client.email;
            valueOfPlaceOfBirth_Label.Content = client.placeOfBirth;
            valueOfResidentialAddress_Label.Content = client.residentialAddress;
            valueOfCorrespondencyAddress_Label.Content = client.correspondencyAddress;
            valueOfPasswordReminder_Label.Content = client.passwordReminder;
        }

        private void displayCardData()
        {
            string currency = " PLN";
            double overallValue = client.checkingAccount + client.savingsAccount;
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
                valueOfCheckingAccount_Label.Content = "-" + currency;
                valueOfSavingsAccount_Label.Content = "-" + currency;
                overallValue_Label.Content = "-" + currency;
                valueOfCardStatus_Label.Content = "inactive";
                valueOfCardStatus_Label.Foreground = Brushes.Red;
            }
            else if (client.cardActivity == 1)
            {
                valueOfCheckingAccount_Label.Content = client.checkingAccount.ToString() + currency;
                valueOfSavingsAccount_Label.Content = client.savingsAccount.ToString() + currency;
                overallValue_Label.Content = overallValue.ToString() + currency;
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

        private void goToOrderCardPage(object sender, RoutedEventArgs e)
        {
            ServicesOrderCard servicesOrderCard = new ServicesOrderCard(client);
            servicesOrderCard.Show();
            this.Hide();
        }

        private void goToEditDataPage(object sender, RoutedEventArgs e)
        {
            AccountEditData accountEditData = new AccountEditData(client);
            accountEditData.Show();
            this.Hide();
        }

        private void deleteAccount(object sender, RoutedEventArgs e)
        {
            int clientID = client.id;

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete your account?", "eBank", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sqlQuery = "DELETE FROM clients WHERE id = @clientID";
                        SqlCommand command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.AddWithValue("@clientID", clientID);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Account successfully removed.", "eBank");
                        }
                        else
                        {
                            MessageBox.Show("Account not found.", "eBank");
                        }

                        connection.Close();

                        MainWindow loginPage = new MainWindow();
                        loginPage.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error removing profile: " + ex.Message, "eBank");
                }
            }
            else
            {
                InvalidateVisual();
            }
        }
    }
}
