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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace eBank
{
    /// <summary>
    /// Logika interakcji dla klasy HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client = null;
        public HomePage(Client _client)
        {
            client = _client;
            InitializeComponent();
            displayData();
        }

        private void displayData()
        {
            date_Label.Content = DateTime.Now.ToString("yyyy-MM-dd");
            name_Label.Content = client.name;
            displayCardsData();
            calculateIncomeAndExpenses();
            displayTransactions();
        }

        private void displayCardsData()
        {
            //CARDS COLOR
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
            if(client.cardActivity == 1)
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

            //SAVINGS ACCOUNT
            if (client.cardActivity == 1)
            {
                valueOfSavingsAccount_Label.Content = client.savingsAccount + currency;

            }
            else
            {
                valueOfSavingsAccount_Label.Content = "-" + currency;
            }

            //OVERALL VALUE
            string overallString = "Overall: ";
            double overallValue = client.checkingAccount + client.savingsAccount;
            if (client.cardActivity == 1)
            {
                overallValue_Label.Content = overallString + overallValue + currency;

            }
            else
            {
                overallValue_Label.Content = overallString + "-" + currency;

            }
        }

        private void calculateIncomeAndExpenses()
        {
            int clientID = client.id;
            int depositID = 1;
            int withdrawID = 2;
            int ownTransferID = 4;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                try
                {
                    connection.Open();

                    // Income 
                    command.CommandText = "SELECT SUM(value) FROM transactions WHERE ((senderID != @clientID AND recipientID = @clientID) OR " +
                                          "(senderID = @clientID AND recipientID = @clientID AND type = @depositID))";
                    command.Parameters.AddWithValue("@clientID", clientID);
                    command.Parameters.AddWithValue("@depositID", depositID);

                    object incomeResult = command.ExecuteScalar();
                    if (incomeResult != DBNull.Value)
                    {
                        decimal income = Convert.ToDecimal(incomeResult);
                        income_Label.Content = "+" + income.ToString() + " PLN";
                    }
                    else
                    {
                        income_Label.Content = "0,00 PLN";
                    }

                    // Expenses
                    command.CommandText = "SELECT SUM(value) FROM transactions WHERE ((senderID = @clientID AND recipientID != @clientID) OR " +
                                          "(senderID = @clientID AND recipientID = @clientID AND type = @withdrawID) OR " +
                                          "(senderID = @clientID AND recipientID = @clientID AND type != @ownTransferID))";
                    command.Parameters.AddWithValue("@withdrawID", withdrawID);
                    command.Parameters.AddWithValue("@ownTransferID", ownTransferID);

                    object expensesResult = command.ExecuteScalar();
                    if (expensesResult != DBNull.Value)
                    {
                        decimal expenses = Convert.ToDecimal(expensesResult);
                        expenses_Label.Content = "-" + expenses.ToString() + " PLN";
                    }
                    else
                    {
                        expenses_Label.Content = "0,00 PLN";
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error: " + ex.Message, "eBank");
                }
            }
        }

        private void displayTransactions()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int depositID = 1;
                    int withdrawID = 2;
                    int ownTransferID = 4;
                    int clientID = client.id;

                    SqlCommand command = new SqlCommand("SELECT CONVERT(VARCHAR(10), [transactions].[date], 120) AS 'Date', " +
                        "[transactions].[id] AS 'Transaction number', " +
                        "[transactionType].[name] AS 'Type', " +
                        "[transactions].[senderID] AS 'SenderID', " +
                        "[transactions].[recipientID] AS 'RecipientID', " +
                        "CASE " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] != @clientID THEN '-' + CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "WHEN [transactions].[recipientID] = @clientID AND [transactions].[senderID] != @clientID THEN '+' + CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @depositID THEN '+' + CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @withdrawID THEN '-' + CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @ownTransferID THEN CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "ELSE '-' + CAST([transactions].[value] AS VARCHAR) + ' PLN' END AS 'Value' " +
                        "FROM transactions " +
                        "INNER JOIN transactionType ON [transactions].[type] = [transactionType].[id] " +
                        "WHERE ([transactions].[senderID] = @clientID OR [transactions].[recipientID] = @clientID) " +
                        "ORDER BY [transactions].[date] DESC", connection);

                    command.Parameters.AddWithValue("@depositID", depositID);
                    command.Parameters.AddWithValue("@withdrawID", withdrawID);
                    command.Parameters.AddWithValue("@ownTransferID", ownTransferID);
                    command.Parameters.AddWithValue("@clientID", clientID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int recordCount = 1;

                        while (reader.Read() && recordCount <= 3)
                        {
                            string transactionDate = reader["Date"].ToString();
                            string transactionType = reader["Type"].ToString();
                            string transactionValue = reader["Value"].ToString();
                            int senderID = reader.GetInt32(reader.GetOrdinal("SenderID"));
                            int recipientID = reader.GetInt32(reader.GetOrdinal("RecipientID"));


                            if (recordCount == 1)
                            {
                                transactionDate1_Label.Content = transactionDate;
                                transactionType1_Label.Content = transactionType;
                                transactionValue1_Label.Content = transactionValue;

                                // Color arrows
                                if ((senderID == clientID && recipientID == clientID && transactionType == "Own transfer"))
                                {
                                    rightArrow1_Label.Foreground = Brushes.Black;
                                    leftArrow1_Label.Foreground = Brushes.Black;
                                    transactionValue1_Label.Background = new SolidColorBrush(Color.FromRgb(206, 206, 206));
                                }
                                else if ((senderID != clientID && recipientID == clientID) ||
                                    (senderID == clientID && recipientID == clientID && transactionType == "Deposit"))
                                {
                                    rightArrow1_Label.Foreground = Brushes.LimeGreen;
                                    transactionValue1_Label.Background = new SolidColorBrush(Color.FromRgb(204, 255, 192));
                                }
                                else if ((senderID == clientID && recipientID != clientID) ||
                                    (senderID == clientID && recipientID == clientID && transactionType == "Withdraw"))
                                {
                                    leftArrow1_Label.Foreground = Brushes.Red;
                                    transactionValue1_Label.Background = new SolidColorBrush(Color.FromRgb(240, 167, 167));
                                }
                                else
                                {
                                    leftArrow1_Label.Foreground = Brushes.Red;
                                    transactionValue1_Label.Background = new SolidColorBrush(Color.FromRgb(240, 167, 167));
                                }
                            }
                            else if (recordCount == 2)
                            {
                                transactionDate2_Label.Content = transactionDate;
                                transactionType2_Label.Content = transactionType;
                                transactionValue2_Label.Content = transactionValue;

                                if ((senderID == clientID && recipientID == clientID && transactionType == "Own transfer"))
                                {
                                    rightArrow2_Label.Foreground = Brushes.Black;
                                    leftArrow2_Label.Foreground = Brushes.Black;
                                    transactionValue2_Label.Background = new SolidColorBrush(Color.FromRgb(206, 206, 206));
                                }
                                else if ((senderID != clientID && recipientID == clientID) ||
                                    (senderID == clientID && recipientID == clientID && transactionType == "Deposit"))
                                {
                                    rightArrow2_Label.Foreground = Brushes.LimeGreen;
                                    transactionValue2_Label.Background = new SolidColorBrush(Color.FromRgb(204, 255, 192));
                                }
                                else if ((senderID == clientID && recipientID != clientID) ||
                                    (senderID == clientID && recipientID == clientID && transactionType == "Withdraw"))
                                {
                                    leftArrow2_Label.Foreground = Brushes.Red;
                                    transactionValue2_Label.Background = new SolidColorBrush(Color.FromRgb(240, 167, 167));
                                }
                                else
                                {
                                    leftArrow2_Label.Foreground = Brushes.Red;
                                    transactionValue2_Label.Background = new SolidColorBrush(Color.FromRgb(240, 167, 167));
                                }
                            }
                            else // recordCount == 3
                            {
                                transactionDate3_Label.Content = transactionDate;
                                transactionType3_Label.Content = transactionType;
                                transactionValue3_Label.Content = transactionValue;

                                if ((senderID == clientID && recipientID == clientID && transactionType == "Own transfer"))
                                {
                                    rightArrow3_Label.Foreground = Brushes.Black;
                                    leftArrow3_Label.Foreground = Brushes.Black;
                                    transactionValue3_Label.Background = new SolidColorBrush(Color.FromRgb(206, 206, 206));
                                }
                                else if ((senderID != clientID && recipientID == clientID) ||
                                    (senderID == clientID && recipientID == clientID && transactionType == "Deposit"))
                                {
                                    rightArrow3_Label.Foreground = Brushes.LimeGreen;
                                    transactionValue3_Label.Background = new SolidColorBrush(Color.FromRgb(204, 255, 192));
                                }
                                else if ((senderID == clientID && recipientID != clientID) ||
                                    (senderID == clientID && recipientID == clientID && transactionType == "Withdraw"))
                                {
                                    leftArrow3_Label.Foreground = Brushes.Red;
                                    transactionValue3_Label.Background = new SolidColorBrush(Color.FromRgb(240, 167, 167));
                                }
                                else
                                {
                                    leftArrow3_Label.Foreground = Brushes.Red;
                                    transactionValue3_Label.Background = new SolidColorBrush(Color.FromRgb(240, 167, 167));
                                }
                            }

                            recordCount++;
                        }

                        // If no transaction was found, clear the labels
                        for (int i = recordCount; i <= 3; i++)
                        {
                            if (i == 1)
                            {
                                transactionDate1_Label.Content = "No transaction found.";
                                transactionType1_Label.Content = "";
                                transactionValue1_Label.Content = "- PLN";
                            }
                            else if (i == 2)
                            {
                                transactionDate2_Label.Content = "No transaction found.";
                                transactionType2_Label.Content = "";
                                transactionValue2_Label.Content = "- PLN";
                            }
                            else // recordCount == 3
                            {
                                transactionDate3_Label.Content = "No transaction found.";
                                transactionType3_Label.Content = "";
                                transactionValue3_Label.Content = "- PLN";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error: " + ex.Message, "eBank");
                }
            }
        }

        private void goToHomePage(object sender, RoutedEventArgs e)
        {
            InvalidateVisual();
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
            System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you want to log out?", "eBank", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                MainWindow loginPage = new MainWindow();
                loginPage.Show();
                this.Hide();
            }
        }
    }
}
