using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
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

        private void downloadTransactionConfirmation(object sender, RoutedEventArgs e)
        {
            string pdfFilePath = "Transaction confirmation.pdf";
            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont logoFont = new XFont("Eras ITC", 16, XFontStyle.Bold);
            XFont TransactionConfirmationFont = new XFont("Calibri", 20);
            XFont documentGeneratedFont = new XFont("Calibri Light", 11);
            XFont detailsFont = new XFont("Calibri", 11);
            XFont titleFont = new XFont("Calibri", 13, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);

            //Data to be saved in a PDF file
            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("yyyy-MM-dd, hh:mm:ss");

            string senderCardNumber = fromSenderAccount_Label.Content.ToString();
            string senderFullName = senderSurnameAndName_Label.Content.ToString();
            string senderResidentialAddress = senderResidentialAddress_Label.Content.ToString();

            string recipientCardNumber = onRecipientAccount_Label.Content.ToString();
            string recipientFullName = recipientSurnameAndName_Label.Content.ToString();
            string recipientResidentialAddress = recipientResidentialAddress_Label.Content.ToString();

            string type = type_Label.Content.ToString();
            string title = title_Label.Content.ToString();
            string description = description_Label.Content.ToString();
            string date = transactionDate_Label.Content.ToString();
            string value = value_Label.Content.ToString();
            string transactionNumber = transactionNumber_Label.Content.ToString();

            // Logo and title
            tf.DrawString("eBank", logoFont, XBrushes.Black, new XRect(50, 50, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Transaction confirmation", TransactionConfirmationFont, XBrushes.Black, new XRect(50, 70, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Document generated: " + formattedDate, documentGeneratedFont, XBrushes.Black, new XRect(50, 95, page.Width - 100, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, new XPoint(50, 110), new XPoint(page.Width - 100, 110));

            // Payer details
            tf.DrawString("Payer details:", titleFont, XBrushes.Black, new XRect(50, 120, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Card Number: " + senderCardNumber, detailsFont, XBrushes.Black, new XRect(50, 140, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Surname and name: " + senderFullName, detailsFont, XBrushes.Black, new XRect(50, 160, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Residential Address: " + senderResidentialAddress, detailsFont, XBrushes.Black, new XRect(50, 180, page.Width - 100, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, new XPoint(50, 200), new XPoint(page.Width - 100, 200));

            // Recipient's details
            tf.DrawString("Recipient's details:", titleFont, XBrushes.Black, new XRect(50, 210, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Card Number: " + recipientCardNumber, detailsFont, XBrushes.Black, new XRect(50, 230, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Surname and name: " + recipientFullName, detailsFont, XBrushes.Black, new XRect(50, 250, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Residential Address: " + recipientResidentialAddress, detailsFont, XBrushes.Black, new XRect(50, 270, page.Width - 100, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, new XPoint(50, 290), new XPoint(page.Width - 100, 290));

            // Transfer details
            tf.DrawString("Transfer details:", titleFont, XBrushes.Black, new XRect(50, 300, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Type: " + type, detailsFont, XBrushes.Black, new XRect(50, 320, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Title: " + title, detailsFont, XBrushes.Black, new XRect(50, 340, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Description: " + description, detailsFont, XBrushes.Black, new XRect(50, 360, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Date: " + date, detailsFont, XBrushes.Black, new XRect(50, 380, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Value: " + value, detailsFont, XBrushes.Black, new XRect(50, 400, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Transaction Number: " + transactionNumber, detailsFont, XBrushes.Black, new XRect(50, 420, page.Width - 100, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, new XPoint(50, 440), new XPoint(page.Width - 100, 440));

            document.Save(pdfFilePath);
            System.Diagnostics.Process.Start(pdfFilePath);
        }

        private void backToHistoryPage(object sender, RoutedEventArgs e)
        {
            HistoryPage historyPage = new HistoryPage(client);
            historyPage.Show();
            this.Hide();
        }
    }
}
