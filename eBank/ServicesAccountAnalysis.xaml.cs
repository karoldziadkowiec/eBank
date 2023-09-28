using PdfSharp.Charting;
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
    /// Logika interakcji dla klasy ServicesAccountAnalysis.xaml
    /// </summary>
    public partial class ServicesAccountAnalysis : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client;
        public ServicesAccountAnalysis(Client _client)
        {
            client = _client;
            InitializeComponent();
            displayData();
        }

        private void displayData()
        {
            date_Label.Content = DateTime.Now.ToString("yyyy-MM-dd");
            numberOfDays_ComboBox.Items.Add(3);
            numberOfDays_ComboBox.Items.Add(7);
            numberOfDays_ComboBox.Items.Add(30);
            numberOfDays_ComboBox.Items.Add(90);
            numberOfDays_ComboBox.Items.Add(180);
            numberOfDays_ComboBox.Items.Add(365);
            numberOfDays_ComboBox.SelectedItem = 30;
            displayCardData();
            calculateMonthlyIncomeAndExpenses();
            calculateOverallIncomeAndExpenses();
        }

        private void displayCardData()
        {
            string currency = " PLN";
            double overallValue = client.checkingAccount + client.savingsAccount;

            if (client.cardActivity == 0)
            {
                valueOfCheckingAccount_Label.Content = "-" + currency;
                valueOfSavingsAccount_Label.Content = "-" + currency;
                overallValue_Label.Content = "-" + currency;
            }
            else if (client.cardActivity == 1)
            {
                valueOfCheckingAccount_Label.Content = client.checkingAccount.ToString() + currency;
                valueOfSavingsAccount_Label.Content = client.savingsAccount.ToString() + currency;
                overallValue_Label.Content = overallValue.ToString() + currency;
            }
        }

        private void calculateMonthlyIncomeAndExpenses()
        {
            int clientID = client.id;
            int depositID = 1;
            int withdrawID = 2;
            int ownTransferID = 4;
            DateTime thirtyDaysAgo = DateTime.Now.AddDays(-30);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                try
                {
                    connection.Open();

                    // Income 
                    command.CommandText = "SELECT SUM(value) FROM transactions WHERE ((senderID != @clientID AND recipientID = @clientID) OR " +
                                          "(senderID = @clientID AND recipientID = @clientID AND type = @depositID)) " +
                                          "AND [transactions].[date] >= @thirtyDaysAgo";
                    command.Parameters.AddWithValue("@clientID", clientID);
                    command.Parameters.AddWithValue("@depositID", depositID);
                    command.Parameters.AddWithValue("@thirtyDaysAgo", thirtyDaysAgo);

                    object incomeResult = command.ExecuteScalar();
                    if (incomeResult != DBNull.Value)
                    {
                        decimal income = Convert.ToDecimal(incomeResult);
                        income2_Label.Content = "+" + income.ToString() + " PLN";
                    }
                    else
                    {
                        income2_Label.Content = "0,00 PLN";
                    }

                    // Expenses
                    command.CommandText = "SELECT SUM(value) FROM transactions " +
                                          "WHERE ((senderID = @clientID AND recipientID != @clientID) OR " +
                                          "(senderID = @clientID AND recipientID = @clientID AND type = @withdrawID) OR " +
                                          "(senderID = @clientID AND recipientID = @clientID AND (type != @ownTransferID AND type != @depositID))) " +
                                          "AND [transactions].[date] >= @thirtyDaysAgo";
                    command.Parameters.AddWithValue("@withdrawID", withdrawID);
                    command.Parameters.AddWithValue("@ownTransferID", ownTransferID);

                    object expensesResult = command.ExecuteScalar();
                    if (expensesResult != DBNull.Value)
                    {
                        decimal expenses = Convert.ToDecimal(expensesResult);
                        expenses2_Label.Content = "-" + expenses.ToString() + " PLN";
                    }
                    else
                    {
                        expenses2_Label.Content = "0,00 PLN";
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error: " + ex.Message, "eBank");
                }
            }
        }

        private void calculateOverallIncomeAndExpenses()
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
                                          "(senderID = @clientID AND recipientID = @clientID AND (type != @ownTransferID AND type != @depositID)))";
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

        private void filterTransations(object sender, RoutedEventArgs e)
        {
            int clientID = client.id;
            int depositID = 1;
            int withdrawID = 2;
            int ownTransferID = 4;
            if (int.TryParse(numberOfDays_ComboBox.Text, out int numberOfDays))
            {
            }
            DateTime xDaysAgo = DateTime.Now.AddDays(-numberOfDays);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                try
                {
                    connection.Open();

                    // Income 
                    command.CommandText = "SELECT SUM(value) FROM transactions WHERE ((senderID != @clientID AND recipientID = @clientID) OR " +
                                          "(senderID = @clientID AND recipientID = @clientID AND type = @depositID)) " +
                                          "AND [transactions].[date] >= @xDaysAgo";
                    command.Parameters.AddWithValue("@clientID", clientID);
                    command.Parameters.AddWithValue("@depositID", depositID);
                    command.Parameters.AddWithValue("@xDaysAgo", xDaysAgo);

                    object incomeResult = command.ExecuteScalar();
                    if (incomeResult != DBNull.Value)
                    {
                        decimal income = Convert.ToDecimal(incomeResult);
                        income2_Label.Content = "+" + income.ToString() + " PLN";
                    }
                    else
                    {
                        income2_Label.Content = "0,00 PLN";
                    }

                    // Expenses
                    command.CommandText = "SELECT SUM(value) FROM transactions " +
                                          "WHERE ((senderID = @clientID AND recipientID != @clientID) OR " +
                                          "(senderID = @clientID AND recipientID = @clientID AND type = @withdrawID) OR " +
                                          "(senderID = @clientID AND recipientID = @clientID AND (type != @ownTransferID AND type != @depositID))) " +
                                          "AND [transactions].[date] >= @xDaysAgo";
                    command.Parameters.AddWithValue("@withdrawID", withdrawID);
                    command.Parameters.AddWithValue("@ownTransferID", ownTransferID);

                    object expensesResult = command.ExecuteScalar();
                    if (expensesResult != DBNull.Value)
                    {
                        decimal expenses = Convert.ToDecimal(expensesResult);
                        expenses2_Label.Content = "-" + expenses.ToString() + " PLN";
                    }
                    else
                    {
                        expenses2_Label.Content = "0,00 PLN";
                    }

                    InvalidateVisual();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error: " + ex.Message, "eBank");
                }
            }
        }
    }
}
