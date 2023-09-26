using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logika interakcji dla klasy ServicesTransferRequest.xaml
    /// </summary>
    public partial class ServicesTransferRequest : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client;
        public ServicesTransferRequest(Client _client)
        {
            client = _client;
            InitializeComponent();
            displayData();
        }

        private void displayData()
        {
            date_Label.Content = DateTime.Now.ToString("yyyy-MM-dd");
            displayCardData();
            loadTransferRequestsData();
        }

        private void loadTransferRequestsData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int clientID = client.id;

                    SqlCommand cmdDataBase = new SqlCommand("SELECT TR.id AS 'Request number', C.surname + ' ' + C.name AS 'From', CAST(TR.value AS VARCHAR) + ' PLN' AS 'Value', TR.title AS 'Title' " +
                        "FROM transferRequests AS TR " +
                        "INNER JOIN clients AS C ON TR.senderID = C.id " +
                        "WHERE TR.recipientID = @clientID " +
                        "ORDER BY TR.date DESC", connection);
                    cmdDataBase.Parameters.AddWithValue("@clientID", clientID);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmdDataBase);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    incomingRequestsTable_DataGrid.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "eBank");
                }
            }
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

        private void sendRequest(object sender, RoutedEventArgs e)
        {
            int senderID = client.id;
            int recipientID = 0;
            string recipientCardNumber = accountNumber_TextBox.Text;
            double value;
            string title = title_TextBox.Text;
            string dateOfRequest = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (string.IsNullOrEmpty(recipientCardNumber) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(value_TextBox.Text))
            {
                MessageBox.Show("Complete the empty fields.", "eBank");
                return;
            }

            if (!Double.TryParse(value_TextBox.Text, out value))
            {
                MessageBox.Show("Invalid withdrawal limit format. Please enter a valid value (0,00).", "eBank");
                return;
            }

            if (value <= 0)
            {
                MessageBox.Show("Value must be a positive number.", "eBank");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // GET recipientID
                    string getRecipientIDQuery = "SELECT id FROM clients WHERE cardNumber = @recipientCardNumber";
                    SqlCommand getRecipientIDCommand = new SqlCommand(getRecipientIDQuery, connection);
                    getRecipientIDCommand.Parameters.AddWithValue("@recipientCardNumber", recipientCardNumber);
                    object recipientIDResult = getRecipientIDCommand.ExecuteScalar();
                    if (recipientIDResult != null)
                    {
                        recipientID = (int)recipientIDResult;
                    }

                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        // updating transferRequests table
                        string sqlQuery = "INSERT INTO transferRequests (senderID, recipientID, value, title, date) VALUES (@senderID, @recipientID, @value, @title, @dateOfRequest)";
                        SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);
                        command.Parameters.AddWithValue("@senderID", senderID);
                        command.Parameters.AddWithValue("@recipientID", recipientID);
                        command.Parameters.AddWithValue("@value", value);
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@dateOfRequest", dateOfRequest);
                        command.ExecuteNonQuery();
                        transaction.Commit();

                        MessageBox.Show("The transfer request has been sent.", "eBank");
                        connection.Close();

                        HomePage homePage = new HomePage(client);
                        homePage.Show();
                        this.Hide();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Transfer request error. Entered client does not exist.", "eBank");
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Connection error.", "eBank");
            }
        }

        private void backToServicesPage(object sender, RoutedEventArgs e)
        {
            ServicesPage servicesPage = new ServicesPage(client);
            servicesPage.Show();
            this.Hide();
        }

        private void acceptRequest(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(requestNumber_TextBox.Text))
            {
                MessageBox.Show("Please enter the request number.", "eBank");
                return;
            }

            if (int.TryParse(requestNumber_TextBox.Text, out int requestID))
            {
                string transferType = "Transfer request";
                int TransferTypeID = 0;
                int senderID = 0;
                int recipientID = 0;
                double value = 0.0;
                string title = "";
                string description = "-";
                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        SqlCommand cmdDataBase = new SqlCommand("SELECT senderID, recipientID, value, title " +
                            "FROM transferRequests " +
                            "WHERE id = @requestID", connection);
                        cmdDataBase.Parameters.AddWithValue("@requestID", requestID);

                        SqlDataReader reader = cmdDataBase.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            senderID = reader.GetInt32(0);
                            recipientID = reader.GetInt32(1);
                            value = reader.GetDouble(2);
                            title = reader.GetString(3);

                            if (value > client.checkingAccount)
                            {
                                MessageBox.Show("You don't have this value in your checking account.", "eBank");
                                reader.Close();
                                return;
                            }

                            if (value > client.transactionLimit)
                            {
                                MessageBox.Show("The entered value is over the set transfer limit.", "eBank");
                                reader.Close();
                                return;
                            }

                            reader.Close();

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
                                string updateSenderAccountQuery = "UPDATE clients SET checkingAccount = checkingAccount - @value WHERE id = @recipientID";
                                SqlCommand updateSenderAccountCommand = new SqlCommand(updateSenderAccountQuery, connection, transaction);
                                updateSenderAccountCommand.Parameters.AddWithValue("@recipientID", recipientID);
                                updateSenderAccountCommand.Parameters.AddWithValue("@value", value);
                                updateSenderAccountCommand.ExecuteNonQuery();

                                // +
                                string updateRecipientAccountQuery = "UPDATE clients SET checkingAccount = checkingAccount + @value WHERE id = @senderID";
                                SqlCommand updateRecipientAccountCommand = new SqlCommand(updateRecipientAccountQuery, connection, transaction);
                                updateRecipientAccountCommand.Parameters.AddWithValue("@senderID", senderID);
                                updateRecipientAccountCommand.Parameters.AddWithValue("@value", value);
                                updateRecipientAccountCommand.ExecuteNonQuery();

                                // PUT NEW TRANSACTION
                                string sqlQuery = "INSERT INTO transactions (senderID, recipientID, value, type, title, description, date) VALUES (@recipientID, @senderID, @value, @type, @title, @description, @date)";
                                SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);
                                command.Parameters.AddWithValue("@recipientID", recipientID);
                                command.Parameters.AddWithValue("@senderID", senderID);
                                command.Parameters.AddWithValue("@value", value);
                                command.Parameters.AddWithValue("@type", TransferTypeID);
                                command.Parameters.AddWithValue("@title", title);
                                command.Parameters.AddWithValue("@description", description);
                                command.Parameters.AddWithValue("@date", date);
                                command.ExecuteNonQuery();
                                transaction.Commit();

                                // DELETE TRANSFER REQUEST
                                string deleteTransferRequestQuery = "DELETE FROM transferRequests WHERE id = @requestID";
                                SqlCommand deleteTransferRequestCommand = new SqlCommand(deleteTransferRequestQuery, connection);
                                deleteTransferRequestCommand.Parameters.AddWithValue("@requestID", requestID);
                                deleteTransferRequestCommand.ExecuteNonQuery();

                                // SET NEW CHECKING ACCOUNT VALUE
                                string getCheckingAccountValueQuery = "SELECT checkingAccount FROM clients WHERE id = @clientID";
                                SqlCommand getCheckingAccountValueCommand = new SqlCommand(getCheckingAccountValueQuery, connection);
                                getCheckingAccountValueCommand.Parameters.AddWithValue("@clientID", client.id);
                                object checkingAccountValue = getCheckingAccountValueCommand.ExecuteScalar();
                                if (checkingAccountValue != null)
                                {
                                    client.checkingAccount = (double)checkingAccountValue;
                                }

                                MessageBox.Show("The transfer request has been completed.", "eBank");
                                connection.Close();

                                HomePage homePage = new HomePage(client);
                                homePage.Show();
                                this.Hide();
                            }
                            catch (SqlException ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Transfer error. The entered client does not exist.", "eBank");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Enter the correct request ID.", "eBank");
                            reader.Close();
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error");
                }
            }
            else
            {
                MessageBox.Show("Invalid request ID. Please enter a valid numeric value.", "eBank");
            }
        }

        private void rejectRequest(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(requestNumber_TextBox.Text))
            {
                MessageBox.Show("Please enter the request number.", "eBank");
                return;
            }

            if (int.TryParse(requestNumber_TextBox.Text, out int requestID))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        // DELETE TRANSFER REQUEST
                        string deleteTransferRequestQuery = "DELETE FROM transferRequests WHERE id = @requestID";
                        SqlCommand deleteTransferRequestCommand = new SqlCommand(deleteTransferRequestQuery, connection);
                        deleteTransferRequestCommand.Parameters.AddWithValue("@requestID", requestID);
                        int rowsAffected = deleteTransferRequestCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("The transfer request was rejected.", "eBank");
                        }
                        else
                        {
                            MessageBox.Show("The transfer request not found.", "eBank");
                        }

                        connection.Close();

                        ServicesTransferRequest transferRequest = new ServicesTransferRequest(client);
                        transferRequest.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error rejecting transfer request: " + ex.Message, "eBank");
                }
            }
            else
            {
                MessageBox.Show("Invalid request number. Please enter a valid numeric value.", "eBank");
            }
        }

        private void selectAndShowDataInTextBox(object sender, SelectionChangedEventArgs e)
        {
            if (incomingRequestsTable_DataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)incomingRequestsTable_DataGrid.SelectedItem;
                string selectedValue = selectedRow["Request number"].ToString();
                requestNumber_TextBox.Text = selectedValue;
            }
        }
    }
}
