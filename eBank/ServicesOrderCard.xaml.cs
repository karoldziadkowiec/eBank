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
    /// Logika interakcji dla klasy ServicesOrderCard.xaml
    /// </summary>
    public partial class ServicesOrderCard : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client = null;
        public ServicesOrderCard(Client _client)
        {
            client = _client;
            InitializeComponent();
            displayData();
        }

        private void displayData()
        {
            date_Label.Content = DateTime.Now.ToString("yyyy-MM-dd");
            if (client.cardNumber == "") {
                cardNumber_Label.Content = "XXXX XXXX XXXX XXXX";
            }
            else { 
                cardNumber_Label.Content = client.cardNumber; 
            }
            cardColor_ComboBox.Items.Add("Brown");
            cardColor_ComboBox.Items.Add("Black");
            cardColor_ComboBox.Items.Add("Green");
            cardColor_ComboBox.SelectedItem = "Brown";
            correspondencyAddress_TextBox.Text = client.correspondencyAddress;
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

        private void generateCardNumber(object sender, RoutedEventArgs e)
        {
            string cardNumberString = generateUniqueCardNumber();

            if (string.IsNullOrEmpty(cardNumberString))
            {
                MessageBox.Show("Unable to generate a unique card number. Please try again later.", "eBank");
                return;
            }

            cardNumber_Label.Content = cardNumberString;
        }

        private string generateUniqueCardNumber()
        {
            while (true)
            {
                long cardNumber = generateRandomCardNumber();
                string cardNumberString = cardNumber.ToString("D16");

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT COUNT(*) FROM clients WHERE cardNumber = @cardNumber";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@cardNumber", cardNumberString);

                            int count = (int)command.ExecuteScalar();

                            if (count == 0)
                            {
                                return cardNumberString;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking card number in the database: " + ex.Message, "eBank");
                    return null;
                }
            }
        }

        private long generateRandomCardNumber()
        {
            Random random = new Random();
            double randomDouble = random.NextDouble() * (9999999999999999 - 1000000000000000) + 1000000000000000;
            return (long)Math.Floor(randomDouble);
        }

        private void orderCard(object sender, RoutedEventArgs e)
        {
            string cardNumber = cardNumber_Label.Content.ToString();
            string cardColor = cardColor_ComboBox.Text;
            string correspondencyAddress = correspondencyAddress_TextBox.Text;
            int cardActivity = 1;
            string cardStartDate = DateTime.Today.ToString("yyyy-MM-dd");
            DateTime cardEndDateDateTime = DateTime.Today.AddYears(10);
            string cardEndDate = cardEndDateDateTime.ToString("yyyy-MM-dd");

            MessageBoxResult result = MessageBox.Show("Are you sure you want to order a new card?", "eBank", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string updateQuery = "UPDATE clients SET cardNumber = @cardNumber, cardColor = @cardColor, correspondenceAddress = @correspondencyAddress, cardActivity = @cardActivity, cardStartDate = @cardStartDate, cardEndDate = @cardEndDate WHERE login = @login";

                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@cardNumber", cardNumber);
                            command.Parameters.AddWithValue("@cardColor", cardColor);
                            command.Parameters.AddWithValue("@correspondencyAddress", correspondencyAddress);
                            command.Parameters.AddWithValue("@cardActivity", cardActivity);
                            command.Parameters.AddWithValue("@cardStartDate", cardStartDate);
                            command.Parameters.AddWithValue("@cardEndDate", cardEndDate);
                            command.Parameters.AddWithValue("@login", client.login);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                client.cardNumber = cardNumber;
                                client.cardColor = cardColor;
                                client.correspondencyAddress = correspondencyAddress;
                                client.cardActivity = cardActivity;
                                client.cardStartDate = cardStartDate;
                                client.cardEndDate = cardEndDate;
                                MessageBox.Show("The card has been created and ordered.", "eBank");

                                AccountPage accountPage = new AccountPage(client);
                                accountPage.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Card ordering update failed. Client not found.", "eBank");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error ordering card: " + ex.Message, "eBank");
                }
            }
        }

        private void backToServicesPage(object sender, RoutedEventArgs e)
        {
            ServicesPage servicesPage = new ServicesPage(client);
            servicesPage.Show();
            this.Hide();
        }
    }
}
