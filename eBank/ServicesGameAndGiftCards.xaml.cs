using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
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
    /// Logika interakcji dla klasy ServicesGameAndGiftCards.xaml
    /// </summary>
    public partial class ServicesGameAndGiftCards : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client;
        public ServicesGameAndGiftCards(Client _client)
        {
            client = _client;
            InitializeComponent();
            displayAdminPanel();
            displayData();
        }

        private void displayData()
        {
            date_Label.Content = DateTime.Now.ToString("yyyy-MM-dd");
            displayCardData();
            displayCardTypeInComboBox();
            displayCardValues();
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

        private void displayCardTypeInComboBox()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmdDataBase = new SqlCommand("SELECT name FROM gameAndGiftCards", connection);

                    using (SqlDataReader reader = cmdDataBase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tariffName = reader["name"].ToString();
                            cardType_ComboBox.Items.Add(tariffName);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "eBank");
            }
        }

        private void displayCardValues()
        {
            value_ComboBox.Items.Add(20);
            value_ComboBox.Items.Add(40);
            value_ComboBox.Items.Add(60);
            value_ComboBox.Items.Add(100);
            value_ComboBox.Items.Add(200);
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

        private void payAndGetCard(object sender, RoutedEventArgs e)
        {
            string transferType = "Game/gift card";
            int TransferTypeID = 0;
            int senderID = client.id;
            int recipientID = client.id;
            string cardType = cardType_ComboBox.Text;
            string valueString = value_ComboBox.Text;
            double value;
            string title = title_TextBox.Text;
            string description = "Card: " + cardType;
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (string.IsNullOrEmpty(cardType) || string.IsNullOrEmpty(valueString) || string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Complete the empty fields.", "eBank");
                return;
            }

            if (!Double.TryParse(valueString, out value))
            {
            }

            if (value > client.checkingAccount)
            {
                MessageBox.Show("You don't have this value in your checking account.", "eBank");
                return;
            }

            if (value > client.transactionLimit)
            {
                MessageBox.Show("The entered value is over set transfer limit.", "eBank");
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
                        updateSenderAccountCommand.Parameters.AddWithValue("@senderID", senderID);
                        updateSenderAccountCommand.Parameters.AddWithValue("@value", value);
                        updateSenderAccountCommand.ExecuteNonQuery();

                        // updating transactions table
                        string sqlQuery = "INSERT INTO transactions (senderID, recipientID, value, type, title, description, date) VALUES (@senderID, @recipientID, @value, @type, @title, @description, @date)";
                        SqlCommand command = new SqlCommand(sqlQuery, connection, transaction);
                        command.Parameters.AddWithValue("@senderID", senderID);
                        command.Parameters.AddWithValue("@recipientID", recipientID);
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
                        getCheckingAccountValueCommand.Parameters.AddWithValue("@clientID", client.id);
                        object CheckingAccountValue = getCheckingAccountValueCommand.ExecuteScalar();
                        if (CheckingAccountValue != null)
                        {
                            client.checkingAccount = (double)CheckingAccountValue;
                        }

                        MessageBox.Show("Top-up has been completed.", "eBank");
                        connection.Close();

                        getCardDetails();

                        HomePage homePage = new HomePage(client);
                        homePage.Show();
                        this.Hide();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Top-up error.", "eBank");
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Connection error.", "eBank");
            }
        }

        private void getCardDetails()
        {
            string pdfFilePath = "Card details.pdf";
            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont logoFont = new XFont("Eras ITC", 16, XFontStyle.Bold);
            XFont cardDetailsFont = new XFont("Calibri", 20);
            XFont cardGeneratedFont = new XFont("Calibri Light", 11);
            XFont detailsFont = new XFont("Calibri", 11);
            XFont titleFont = new XFont("Calibri", 13, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);

            //Data to be saved in a PDF file
            DateTime currentDateTime = DateTime.Now;
            string currentDate = currentDateTime.ToString("yyyy-MM-dd");
            DateTime futureDateTime = currentDateTime.AddDays(30);
            string futureDate = futureDateTime.ToString("yyyy-MM-dd");

            string senderCardNumber = client.cardNumber;
            string senderFullName = client.surname + " " + client.name;
            string senderResidentialAddress = client.residentialAddress;

            string cardType = cardType_ComboBox.Text.ToString();
            string value = value_ComboBox.Text.ToString();
            string generatedCardNumber = generateRandomCardNumber();

            // Logo and title
            tf.DrawString("eBank", logoFont, XBrushes.Black, new XRect(50, 50, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Card details", cardDetailsFont, XBrushes.Black, new XRect(50, 70, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Card generated: " + currentDate, cardGeneratedFont, XBrushes.Black, new XRect(50, 95, page.Width - 100, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, new XPoint(50, 110), new XPoint(page.Width - 100, 110));

            // Payer details
            tf.DrawString("Payer details:", titleFont, XBrushes.Black, new XRect(50, 120, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Card Number: " + senderCardNumber, detailsFont, XBrushes.Black, new XRect(50, 140, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Surname and name: " + senderFullName, detailsFont, XBrushes.Black, new XRect(50, 160, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Residential Address: " + senderResidentialAddress, detailsFont, XBrushes.Black, new XRect(50, 180, page.Width - 100, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, new XPoint(50, 200), new XPoint(page.Width - 100, 200));

            // Recipient's details
            tf.DrawString("Card details:", titleFont, XBrushes.Black, new XRect(50, 210, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Generated card number: " + generatedCardNumber, detailsFont, XBrushes.Black, new XRect(50, 230, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Type: " + cardType, detailsFont, XBrushes.Black, new XRect(50, 250, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Value: " + value + " PLN", detailsFont, XBrushes.Black, new XRect(50, 270, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Active until: " + futureDateTime, detailsFont, XBrushes.Black, new XRect(50, 290, page.Width - 100, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, new XPoint(50, 310), new XPoint(page.Width - 100, 310));

            document.Save(pdfFilePath);
            System.Diagnostics.Process.Start(pdfFilePath);
        }

        public static string generateRandomCardNumber()
        {
            Random random = new Random();
            StringBuilder cardNumberBuilder = new StringBuilder();

            for (int i = 0; i < 12; i++)
            {
                int randomNumber = random.Next(10);
                cardNumberBuilder.Append(randomNumber);
            }

            string generatedCardNumber = cardNumberBuilder.ToString();
            return generatedCardNumber;
        }
        private void backToServicesPage(object sender, RoutedEventArgs e)
        {
            ServicesPage servicesPage = new ServicesPage(client);
            servicesPage.Show();
            this.Hide();
        }
    }
}
