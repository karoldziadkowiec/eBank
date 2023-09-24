using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Logika interakcji dla klasy HistoryTransactionDetails.xaml
    /// </summary>
    public partial class HistoryTransactionDetails : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client = null;
        int selectedTransactionNumber = 0;
        public HistoryTransactionDetails(Client _client, int _selectedTransactionNumber)
        {
            client = _client;
            selectedTransactionNumber = _selectedTransactionNumber;
            InitializeComponent();
            displayData();
        }

        private void displayData()
        {
            date_Label.Content = DateTime.Now.ToString("yyyy-MM-dd");
            displayTransactionDetails();
        }

        private void displayTransactionDetails()
        {
            int clientID = client.id;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();

                try
                {
                    connection.Open();

                    string query = "SELECT transactions.*, sender.name AS senderName, sender.surname AS senderSurname, " +
                        "sender.cardNumber AS senderCardNumber, sender.residentialAddress AS senderResidentialAddress, " +
                        "recipient.name AS recipientName, recipient.surname AS recipientSurname, " +
                        "recipient.cardNumber AS recipientCardNumber, recipient.residentialAddress AS recipientResidentialAddress, " +
                        "transactionType.name AS transactionTypeName " +
                        "FROM transactions " +
                        "INNER JOIN clients AS sender ON transactions.senderID = sender.id " +
                        "INNER JOIN clients AS recipient ON transactions.recipientID = recipient.id " +
                        "INNER JOIN transactionType ON transactions.type = transactionType.id " +
                        "WHERE transactions.id = @selectedTransactionNumber";
                    command.CommandText = query;
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@selectedTransactionNumber", selectedTransactionNumber);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int transactionNumber = reader.GetInt32(0);
                                int senderID = reader.GetInt32(1);
                                int recipientID = reader.GetInt32(2);
                                double value = reader.GetDouble(3);
                                int type = reader.GetInt32(4);
                                string title = reader.GetString(5);
                                string description = reader.GetString(6);
                                DateTime date = reader.GetDateTime(7);

                                string senderName = reader.GetString(8);
                                string senderSurname = reader.GetString(9);
                                string senderCardNumber = reader.GetString(10);
                                string senderResidentialAddress = reader.GetString(11);

                                string recipientName = reader.GetString(12);
                                string recipientSurname = reader.GetString(13);
                                string recipientCardNumber = reader.GetString(14);
                                string recipientResidentialAddress = reader.GetString(15);

                                string transactionTypeName = reader.GetString(16);

                                type_Label.Content = transactionTypeName;
                                title_Label.Content = title;
                                description_Label.Content = description;
                                transactionDate_Label.Content = date.ToString("yyyy-MM-dd");
                                string currency = " PLN";
                                value_Label.Content = value + currency;
                                transactionNumber_Label.Content = transactionNumber;

                                fromSenderAccount_Label.Content = senderCardNumber;
                                senderSurnameAndName_Label.Content = $"{senderSurname} {senderName}";
                                senderResidentialAddress_Label.Content = senderResidentialAddress;

                                onRecipientAccount_Label.Content = recipientCardNumber;
                                recipientSurnameAndName_Label.Content = $"{recipientSurname} {recipientName}";
                                recipientResidentialAddress_Label.Content = recipientResidentialAddress;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Transaction not found.", "eBank");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "eBank");
                }
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

        private void downloadTransactionConfirmation(object sender, RoutedEventArgs e)
        {

        }

        private void backToHistoryPage(object sender, RoutedEventArgs e)
        {
            HistoryPage historyPage = new HistoryPage(client);
            historyPage.Show();
            this.Hide();
        }
    }
}
