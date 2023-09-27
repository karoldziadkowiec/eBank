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
    /// Logika interakcji dla klasy ServicesParkingTickets.xaml
    /// </summary>
    public partial class ServicesParkingTickets : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client;
        public ServicesParkingTickets(Client _client)
        {
            client = _client;
            InitializeComponent();
            displayData();
        }

        private void displayData()
        {
            date_Label.Content = DateTime.Now.ToString("yyyy-MM-dd");
            displayCardData();
            displayTicketTimeInComboBox();
            displayCitiesInComboBox();
            showTicketValue();
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

        private void displayTicketTimeInComboBox()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmdDataBase = new SqlCommand("SELECT time FROM parkingTickets", connection);

                    using (SqlDataReader reader = cmdDataBase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tariffName = reader["time"].ToString();
                            time_ComboBox.Items.Add(tariffName);
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

        private void displayCitiesInComboBox()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmdDataBase = new SqlCommand("SELECT name FROM cities", connection);

                    using (SqlDataReader reader = cmdDataBase.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tariffName = reader["name"].ToString();
                            city_ComboBox.Items.Add(tariffName);
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
        private void showTicketValue()
        {
            time_ComboBox.SelectionChanged += TicketTimeComboBox_SelectionChanged;
        }

        private Dictionary<string, string> ticketValues = new Dictionary<string, string>
            {
                { "30 min", "3,00" },
                { "1 h", "6,00" },
                { "2 h", "12,00" },
                { "4 h", "20,00" },
                { "12 h", "60,00" },
                { "1 day", "80,00" },
                { "7 days", "150,00" },
            };

        private void TicketTimeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedTicketTime = time_ComboBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedTicketTime) && ticketValues.ContainsKey(selectedTicketTime))
            {
                value_Label.Content = ticketValues[selectedTicketTime];
            }
            else
            {
                value_Label.Content = "";
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

        private void payAndGetTicket(object sender, RoutedEventArgs e)
        {
            string transferType = "Parking ticket";
            int TransferTypeID = 0;
            int senderID = client.id;
            int recipientID = client.id;
            string ticketTime = time_ComboBox.Text;
            string city = city_ComboBox.Text;
            string valueString = value_Label.Content.ToString();
            double value;
            string title = city;
            string description = "Ticket: " + ticketTime + ", " + city;
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (string.IsNullOrEmpty(ticketTime) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(valueString))
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

                        MessageBox.Show("Payment has been completed.", "eBank");
                        connection.Close();

                        getTicketDetails();

                        HomePage homePage = new HomePage(client);
                        homePage.Show();
                        this.Hide();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Payment error.", "eBank");
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Connection error.", "eBank");
            }
        }

        private void getTicketDetails()
        {
            string pdfFilePath = "Parking ticket details.pdf";
            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont logoFont = new XFont("Eras ITC", 16, XFontStyle.Bold);
            XFont parkingTicketFont = new XFont("Calibri", 20);
            XFont ticketGeneratedFont = new XFont("Calibri Light", 11);
            XFont detailsFont = new XFont("Calibri", 11);
            XFont titleFont = new XFont("Calibri", 13, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);

            //Data to be saved in a PDF file
            DateTime currentDateTime = DateTime.Now;
            string currentDate = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");

            string senderCardNumber = client.cardNumber;
            string senderFullName = client.surname + " " + client.name;
            string senderResidentialAddress = client.residentialAddress;

            string ticketTime = time_ComboBox.Text.ToString();
            string city = city_ComboBox.Text.ToString();
            string valueString = value_Label.Content.ToString();
            double value;
            if (!Double.TryParse(valueString, out value))
            {
            }

            // Logo and title
            tf.DrawString("eBank", logoFont, XBrushes.Black, new XRect(50, 50, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Parking ticket", parkingTicketFont, XBrushes.Black, new XRect(50, 70, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Ticket generated: " + currentDate, ticketGeneratedFont, XBrushes.Black, new XRect(50, 95, page.Width - 100, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, new XPoint(50, 110), new XPoint(page.Width - 100, 110));

            // Payer details
            tf.DrawString("Payer details:", titleFont, XBrushes.Black, new XRect(50, 120, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Card Number: " + senderCardNumber, detailsFont, XBrushes.Black, new XRect(50, 140, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Surname and name: " + senderFullName, detailsFont, XBrushes.Black, new XRect(50, 160, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Residential Address: " + senderResidentialAddress, detailsFont, XBrushes.Black, new XRect(50, 180, page.Width - 100, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, new XPoint(50, 200), new XPoint(page.Width - 100, 200));

            // Recipient's details
            tf.DrawString("Ticket details:", titleFont, XBrushes.Black, new XRect(50, 210, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Time: " + ticketTime, detailsFont, XBrushes.Black, new XRect(50, 230, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("Value: " + value + " PLN", detailsFont, XBrushes.Black, new XRect(50, 250, page.Width - 100, 20), XStringFormats.TopLeft);
            tf.DrawString("City: " + city, detailsFont, XBrushes.Black, new XRect(50, 270, page.Width - 100, 20), XStringFormats.TopLeft);
            gfx.DrawLine(XPens.Black, new XPoint(50, 290), new XPoint(page.Width - 100, 290));

            document.Save(pdfFilePath);
            System.Diagnostics.Process.Start(pdfFilePath);
        }

        private void backToServicesPage(object sender, RoutedEventArgs e)
        {
            ServicesPage servicesPage = new ServicesPage(client);
            servicesPage.Show();
            this.Hide();
        }
    }
}
