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
    /// Logika interakcji dla klasy HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client = null;
        public HistoryPage(Client _client)
        {
            client = _client;
            InitializeComponent();
            displayData();
        }

        private void displayData()
        {
            date_Label.Content = DateTime.Now.ToString("yyyy-MM-dd");
            numberOfDays_ComboBox.Items.Add(30);
            numberOfDays_ComboBox.Items.Add(90);
            numberOfDays_ComboBox.Items.Add(180);
            numberOfDays_ComboBox.Items.Add(365);
            numberOfDays_ComboBox.SelectedItem = 30;
            loadDatabaseData();
        }

        private void loadDatabaseData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int depositID = 1;
                    int withdrawID = 2;
                    DateTime xDaysAgo = DateTime.Now.AddDays(-30);
                    int clientID = client.id;

                    SqlCommand cmdDataBase = new SqlCommand("SELECT CONVERT(VARCHAR(10), [transactions].[date], 120) AS 'Date', " +
                        "[transactions].[id] AS 'Transaction number', " +
                        "CONCAT([transactionType].[name], ': ', [transactions].[title]) AS 'Transaction description', " +
                        "CASE " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] != @clientID THEN '-' + CAST([transactions].[value] AS VARCHAR) " +
                        "WHEN [transactions].[recipientID] = @clientID AND [transactions].[senderID] != @clientID THEN '+' + CAST([transactions].[value] AS VARCHAR) " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @depositID THEN '+' + CAST([transactions].[value] AS VARCHAR) " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @withdrawID THEN '-' + CAST([transactions].[value] AS VARCHAR) " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND ([transactions].[type] != @depositID AND [transactions].[type] != @withdrawID) THEN CAST([transactions].[value] AS VARCHAR) " +
                        "ELSE '-' + CAST([transactions].[value] AS VARCHAR) END AS 'Value' " +
                        "FROM transactions " +
                        "INNER JOIN transactionType ON [transactions].[type] = [transactionType].[id] " +
                        "WHERE [transactions].[date] >= @xDaysAgo " +
                        "AND ([transactions].[senderID] = @clientID OR [transactions].[recipientID] = @clientID) " +
                        "ORDER BY [transactions].[date] DESC", connection);

                    cmdDataBase.Parameters.AddWithValue("@depositID", depositID);
                    cmdDataBase.Parameters.AddWithValue("@withdrawID", withdrawID);
                    cmdDataBase.Parameters.AddWithValue("@xDaysAgo", xDaysAgo);
                    cmdDataBase.Parameters.AddWithValue("@clientID", clientID);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmdDataBase);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    transactionsTable_DataGrid.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
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
            InvalidateVisual();
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int depositID = 1;
                    int withdrawID = 2;
                    if (int.TryParse(numberOfDays_ComboBox.Text, out int numberOfDays))
                    {
                    }
                    DateTime xDaysAgo = DateTime.Now.AddDays(-numberOfDays);
                    int clientID = client.id;

                    SqlCommand cmdDataBase = new SqlCommand("SELECT CONVERT(VARCHAR(10), [transactions].[date], 120) AS 'Date', " +
                        "[transactions].[id] AS 'Transaction number', " +
                        "CONCAT([transactionType].[name], ': ', [transactions].[title]) AS 'Transaction description', " +
                        "CASE " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] != @clientID THEN '-' + CAST([transactions].[value] AS VARCHAR) " +
                        "WHEN [transactions].[recipientID] = @clientID AND [transactions].[senderID] != @clientID THEN '+' + CAST([transactions].[value] AS VARCHAR) " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @depositID THEN '+' + CAST([transactions].[value] AS VARCHAR) " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @withdrawID THEN '-' + CAST([transactions].[value] AS VARCHAR) " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND ([transactions].[type] != @depositID AND [transactions].[type] != @withdrawID) THEN CAST([transactions].[value] AS VARCHAR) " +
                        "ELSE '-' + CAST([transactions].[value] AS VARCHAR) END AS 'Value' " +
                        "FROM transactions " +
                        "INNER JOIN transactionType ON [transactions].[type] = [transactionType].[id] " +
                        "WHERE [transactions].[date] >= @xDaysAgo " +
                        "AND ([transactions].[senderID] = @clientID OR [transactions].[recipientID] = @clientID) " +
                        "ORDER BY [transactions].[date] DESC", connection);

                    cmdDataBase.Parameters.AddWithValue("@depositID", depositID);
                    cmdDataBase.Parameters.AddWithValue("@withdrawID", withdrawID);
                    cmdDataBase.Parameters.AddWithValue("@xDaysAgo", xDaysAgo);
                    cmdDataBase.Parameters.AddWithValue("@clientID", clientID);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmdDataBase);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    transactionsTable_DataGrid.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void downloadHistoryToFile(object sender, RoutedEventArgs e)
        {

        }

        private void goTo(object sender, RoutedEventArgs e)
        {

        }

        private void goToTransactionDetails(object sender, RoutedEventArgs e)
        {
            string selectedTransactionNumberString =  transactionNumber_TextBox.Text;
            int selectedTransactionNumber;
            if (int.TryParse(selectedTransactionNumberString, out selectedTransactionNumber))
            {
            }

            if (!string.IsNullOrEmpty(selectedTransactionNumberString))
            {
                HistoryTransactionDetails transactionDetails = new HistoryTransactionDetails(client, selectedTransactionNumber);
                transactionDetails.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Select a valid transaction number before proceeding.", "eBank");
            }
        }

        private void selectAndShowDataInTextBox(object sender, SelectionChangedEventArgs e)
        {
            if (transactionsTable_DataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)transactionsTable_DataGrid.SelectedItem;
                string selectedValue = selectedRow["Transaction number"].ToString();
                transactionNumber_TextBox.Text = selectedValue;
            }
        }
    }
}
