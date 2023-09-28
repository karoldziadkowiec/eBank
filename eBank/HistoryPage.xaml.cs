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
using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

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
            numberOfDays_ComboBox.Items.Add(7);
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
                    int ownTransferID = 4;
                    DateTime xDaysAgo = DateTime.Now.AddDays(-30);
                    int clientID = client.id;

                    SqlCommand cmdDataBase = new SqlCommand("SELECT CONVERT(VARCHAR(10), [transactions].[date], 120) AS 'Date', " +
                        "[transactions].[id] AS 'Transaction number', " +
                        "CONCAT([transactionType].[name], ': ', [transactions].[title]) AS 'Transaction description', " +
                        "CASE " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] != @clientID THEN '-' + CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "WHEN [transactions].[recipientID] = @clientID AND [transactions].[senderID] != @clientID THEN '+' + CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @depositID THEN '+' + CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @withdrawID THEN '-' + CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @ownTransferID THEN CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "ELSE '-' + CAST([transactions].[value] AS VARCHAR) + ' PLN' END AS 'Value' " +
                        "FROM transactions " +
                        "INNER JOIN transactionType ON [transactions].[type] = [transactionType].[id] " +
                        "WHERE [transactions].[date] >= @xDaysAgo " +
                        "AND ([transactions].[senderID] = @clientID OR [transactions].[recipientID] = @clientID) " +
                        "ORDER BY [transactions].[date] DESC", connection);


                    cmdDataBase.Parameters.AddWithValue("@depositID", depositID);
                    cmdDataBase.Parameters.AddWithValue("@withdrawID", withdrawID);
                    cmdDataBase.Parameters.AddWithValue("@ownTransferID", ownTransferID);
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
                    int ownTransferID = 4;
                    if (int.TryParse(numberOfDays_ComboBox.Text, out int numberOfDays))
                    {
                    }
                    DateTime xDaysAgo = DateTime.Now.AddDays(-numberOfDays);
                    int clientID = client.id;

                    SqlCommand cmdDataBase = new SqlCommand("SELECT CONVERT(VARCHAR(10), [transactions].[date], 120) AS 'Date', " +
                        "[transactions].[id] AS 'Transaction number', " +
                        "CONCAT([transactionType].[name], ': ', [transactions].[title]) AS 'Transaction description', " +
                        "CASE " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] != @clientID THEN '-' + CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "WHEN [transactions].[recipientID] = @clientID AND [transactions].[senderID] != @clientID THEN '+' + CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @depositID THEN '+' + CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @withdrawID THEN '-' + CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "WHEN [transactions].[senderID] = @clientID AND [transactions].[recipientID] = @clientID AND [transactions].[type] = @ownTransferID THEN CAST([transactions].[value] AS VARCHAR) + ' PLN' " +
                        "ELSE '-' + CAST([transactions].[value] AS VARCHAR) + ' PLN' END AS 'Value' " +
                        "FROM transactions " +
                        "INNER JOIN transactionType ON [transactions].[type] = [transactionType].[id] " +
                        "WHERE [transactions].[date] >= @xDaysAgo " +
                        "AND ([transactions].[senderID] = @clientID OR [transactions].[recipientID] = @clientID) " +
                        "ORDER BY [transactions].[date] DESC", connection);


                    cmdDataBase.Parameters.AddWithValue("@depositID", depositID);
                    cmdDataBase.Parameters.AddWithValue("@withdrawID", withdrawID);
                    cmdDataBase.Parameters.AddWithValue("@ownTransferID", ownTransferID);
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
            string pdfFilePath = "Transaction history.pdf";
            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont logoFont = new XFont("Eras ITC", 16, XFontStyle.Bold);
            XFont TransactionHistoryFont = new XFont("Calibri", 20);
            XFont documentGeneratedFont = new XFont("Calibri Light", 11);
            XFont detailsFont = new XFont("Calibri", 11);
            XFont titleFont = new XFont("Calibri", 13, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);

            // Data to be saved in a PDF file
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("yyyy-MM-dd, hh:mm:ss");

            string clientCardNumber = client.cardNumber;
            string fullName = client.surname + " " + client.name;
            string clientFullName = fullName;
            string clientResidentialAddress = client.residentialAddress;

            // Data from DataGrid
            DataTable dataTable = new DataTable();

            foreach (var column in transactionsTable_DataGrid.Columns)
            {
                if (column is DataGridTextColumn textColumn)
                {
                    dataTable.Columns.Add(textColumn.Header.ToString());
                }
            }

            foreach (var item in transactionsTable_DataGrid.Items)
            {
                if (item is DataRowView rowView)
                {
                    DataRow row = dataTable.NewRow();
                    for (int i = 0; i < transactionsTable_DataGrid.Columns.Count; i++)
                    {
                        row[i] = rowView[i].ToString();
                    }
                    dataTable.Rows.Add(row);
                }
            }

            // add logo and title
            tf.DrawString("eBank", logoFont, XBrushes.Black, new XRect(50, 50, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Transaction history", TransactionHistoryFont, XBrushes.Black, new XRect(50, 70, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Document generated: " + formattedDate, documentGeneratedFont, XBrushes.Black, new XRect(50, 95, page.Width - 100, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, new XPoint(50, 110), new XPoint(page.Width - 50, 110));

            // Payer details
            tf.DrawString("Client details:", titleFont, XBrushes.Black, new XRect(50, 120, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Card Number: " + clientCardNumber, detailsFont, XBrushes.Black, new XRect(50, 140, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Surname and name: " + clientFullName, detailsFont, XBrushes.Black, new XRect(50, 160, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Residential Address: " + clientResidentialAddress, detailsFont, XBrushes.Black, new XRect(50, 180, page.Width - 100, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, new XPoint(50, 200), new XPoint(page.Width - 50, 200));

            //Title
            tf.DrawString("Date", titleFont, XBrushes.Black, new XRect(50, 215, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Transaction number", titleFont, XBrushes.Black, new XRect(200, 215, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Transaction description", titleFont, XBrushes.Black, new XRect(350, 215, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Value", titleFont, XBrushes.Black, new XRect(500, 215, page.Width - 100, 20), XStringFormats.TopLeft);

            // Add dataTable to PDF
            int startY = 240;
            int rowHeight = 20;

            foreach (DataRow row in dataTable.Rows)
            {
                int startX = 50;
                foreach (DataColumn column in dataTable.Columns)
                {
                    string cellValue = row[column].ToString();
                    gfx.DrawString(cellValue, detailsFont, XBrushes.Black, new XRect(startX, startY, page.Width - 100, rowHeight), XStringFormats.TopLeft);
                    startX += 150;
                }
                startY += rowHeight;
            }

            document.Save(pdfFilePath);
            System.Diagnostics.Process.Start(pdfFilePath);
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
