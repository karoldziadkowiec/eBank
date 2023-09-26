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
    /// Logika interakcji dla klasy TransfersOwnTransferFromCheckingAccount.xaml
    /// </summary>
    public partial class TransfersOwnTransferFromCheckingAccount : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client;
        public TransfersOwnTransferFromCheckingAccount(Client _client)
        {
            client = _client;
            InitializeComponent();
            displayData();
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
                savingsAccountCard_Rectangle.Fill = Brushes.Black;
            }
            else if (client.cardColor == "Brown")
            {
                checkingAccountCard_Rectangle.Fill = new SolidColorBrush(Color.FromRgb(64, 50, 40));
                savingsAccountCard_Rectangle.Fill = new SolidColorBrush(Color.FromRgb(64, 50, 40));
            }
            else
            {
                checkingAccountCard_Rectangle.Fill = new SolidColorBrush(Color.FromRgb(0, 52, 1));
                savingsAccountCard_Rectangle.Fill = new SolidColorBrush(Color.FromRgb(0, 52, 1));
            }

            string currency = " PLN";
            //CHECKING ACCOUNT
            valueOfCheckingAccount_Label.Content = client.checkingAccount + currency;

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
            if (cardEndDate.Length == 10)
            {
                string firstSevenCharacters = cardEndDate.Substring(0, 7);
                cardEndDate_Label.Content = firstSevenCharacters;
            }
            else
            {
                cardEndDate_Label.Content = "YYYY-MM";
            }

            string fullName = client.name + " " + client.surname;
            nameAndSurname_Label.Content = fullName;

            //SAVINGS ACCOUNT
            valueOfSavingsAccount_Label.Content = client.savingsAccount + currency;
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

        private void sendMoney(object sender, RoutedEventArgs e)
        {
            string transferType = "Own transfer";
            int TransferTypeID = 0;
            int clientID = client.id;
            double value;
            string title = title_TextBox.Text;
            string description = "Own transfer";
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(value_TextBox.Text))
            {
                MessageBox.Show("Complete the empty fields.", "eBank");
                return;
            }

            if (!Double.TryParse(value_TextBox.Text, out value))
            {
                MessageBox.Show("Invalid transfer format. Please enter a valid value (0,00).", "eBank");
                return;
            }

            if (value <= 0)
            {
                MessageBox.Show("Value must be a positive number.", "eBank");
                return;
            }

            if (value > client.checkingAccount)
            {
                MessageBox.Show("You don't have this value in your checking account.", "eBank");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // GET TransferTypeID
                    string getTransferTypeIDQuery = "SELECT id FROM transactionType WHERE name = @transferType";
                    SqlCommand getTransferTypeIDCommand = new SqlCommand(getTransferTypeIDQuery, connection);
                    getTransferTypeIDCommand.Parameters.AddWithValue("@transferType", transferType);
                    object typeIDResult = getTransferTypeIDCommand.ExecuteScalar();
                    if (typeIDResult != null)
                    {
                        TransferTypeID = (int)typeIDResult;
                    }

                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        // -
                        string updateSenderAccountQuery = "UPDATE clients SET checkingAccount = checkingAccount - @value WHERE id = @senderID";
                        SqlCommand updateSenderAccountCommand = new SqlCommand(updateSenderAccountQuery, connection, transaction);
                        updateSenderAccountCommand.Parameters.AddWithValue("@senderID", clientID);
                        updateSenderAccountCommand.Parameters.AddWithValue("@value", value);
                        updateSenderAccountCommand.ExecuteNonQuery();

                        // +
                        string updateRecipientAccountQuery = "UPDATE clients SET savingsAccount = savingsAccount + @value WHERE id = @recipientID";
                        SqlCommand updateRecipientAccountCommand = new SqlCommand(updateRecipientAccountQuery, connection, transaction);
                        updateRecipientAccountCommand.Parameters.AddWithValue("@recipientID", clientID);
                        updateRecipientAccountCommand.Parameters.AddWithValue("@value", value);
                        updateRecipientAccountCommand.ExecuteNonQuery();

                        // updating transactions table
                        string sqlQuery = "INSERT INTO transactions (senderID, recipientID, value, type, title, description, date) VALUES (@senderID, @recipientID, @value, @type, @title, @description, @date)";
                        SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);
                        command.Parameters.AddWithValue("@senderID", clientID);
                        command.Parameters.AddWithValue("@recipientID", clientID);
                        command.Parameters.AddWithValue("@value", value);
                        command.Parameters.AddWithValue("@type", TransferTypeID);
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@date", date);
                        command.ExecuteNonQuery();
                        transaction.Commit();

                        // GET new Checking Account value
                        string getCheckingAccountValueQuery = "SELECT checkingAccount FROM clients WHERE id = @clientID";
                        SqlCommand getCheckingAccountValueCommand = new SqlCommand(getCheckingAccountValueQuery, connection);
                        getCheckingAccountValueCommand.Parameters.AddWithValue("@clientID", clientID);
                        object CheckingAccountValue = getCheckingAccountValueCommand.ExecuteScalar();
                        if (CheckingAccountValue != null)
                        {
                            client.checkingAccount = (double)CheckingAccountValue;
                        }

                        // GET new Savings Account value
                        string getSavingsAccountValueQuery = "SELECT savingsAccount FROM clients WHERE id = @clientID";
                        SqlCommand getSavingsAccountValueCommand = new SqlCommand(getSavingsAccountValueQuery, connection);
                        getSavingsAccountValueCommand.Parameters.AddWithValue("@clientID", clientID);
                        object SavingsAccountValue = getSavingsAccountValueCommand.ExecuteScalar();
                        if (SavingsAccountValue != null)
                        {
                            client.savingsAccount = (double)SavingsAccountValue;
                        }

                        MessageBox.Show("The own transfer has been completed.", "eBank");
                        connection.Close();

                        HomePage homePage = new HomePage(client);
                        homePage.Show();
                        this.Hide();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Own transfer error. Entered client does not exist.", "eBank");
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Connection error.", "eBank");
            }
        }

        private void backToTransfersPage(object sender, RoutedEventArgs e)
        {
            TransfersPage transfersPage = new TransfersPage(client);
            transfersPage.Show();
            this.Hide();
        }

        private void changeCards(object sender, RoutedEventArgs e)
        {
            TransfersOwnTransferFromSavingsAccount ownTransferFromSavingsAccount = new TransfersOwnTransferFromSavingsAccount(client);
            ownTransferFromSavingsAccount.Show();
            this.Hide();
        }
    }
}
