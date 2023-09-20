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
    /// Logika interakcji dla klasy SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client = null;
        public SettingsPage(Client _client)
        {
            client = _client;
            InitializeComponent();
            displayData();
        }

        private void displayData() {
            date_Label.Content = DateTime.Now.ToString("dd.MM.yyyy");
            withdrawalLimit_TextBox.Text = client.withdrawalLimit.ToString();
            transactionLimit_TextBox.Text = client.transactionLimit.ToString();

            if (client.activity == 0)
            {
                accountActivity_Label.Content = "inactive";
                accountActivity_Label.Foreground = Brushes.Red;
            }
            else if (client.activity == 1)
            {
                accountActivity_Label.Content = "active";
                accountActivity_Label.Foreground = Brushes.Green;
            }

            if (client.cardActivity == 0)
            {
                cardActivity_Label.Content = "inactive";
                cardActivity_Label.Foreground = Brushes.Red;
                block_Button.Visibility = Visibility.Hidden;
            }
            else if (client.cardActivity == 1)
            {
                cardActivity_Label.Content = "active";
                cardActivity_Label.Foreground = Brushes.Green;
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
            InvalidateVisual();
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

        private void saveWithdrawalLimit(object sender, RoutedEventArgs e)
        {
            string withdrawalLimitText = withdrawalLimit_TextBox.Text;

            if (string.IsNullOrEmpty(withdrawalLimitText))
            {
                MessageBox.Show("Complete the empty field.", "eBank");
                return;
            }

            if (!double.TryParse(withdrawalLimitText, out double withdrawalLimit))
            {
                MessageBox.Show("Invalid withdrawal limit format. Please enter a valid value (0,00).", "eBank");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE clients SET withdrawalLimit = @withdrawalLimit WHERE login = @login";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@withdrawalLimit", withdrawalLimit);
                        command.Parameters.AddWithValue("@login", client.login);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            client.withdrawalLimit = withdrawalLimit;
                            MessageBox.Show("Withdrawal limit updated successfully.", "eBank");

                            SettingsPage settingsPage = new SettingsPage(client);
                            settingsPage.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Withdrawal limit update failed. Client not found.", "eBank");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating withdrawal limit: " + ex.Message, "eBank");
            }
        }

        private void saveTransactionLimit(object sender, RoutedEventArgs e)
        {
            string transactionLimitText = transactionLimit_TextBox.Text;

            if (string.IsNullOrEmpty(transactionLimitText))
            {
                MessageBox.Show("Complete the empty field.", "eBank");
                return;
            }

            if (!double.TryParse(transactionLimitText, out double transactionLimit))
            {
                MessageBox.Show("Invalid transaction limit format. Please enter a valid value (0,00).", "eBank");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE clients SET transactionLimit = @transactionLimit WHERE login = @login";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@transactionLimit", transactionLimit);
                        command.Parameters.AddWithValue("@login", client.login);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            client.transactionLimit = transactionLimit;
                            MessageBox.Show("Transaction limit updated successfully.", "eBank");

                            SettingsPage settingsPage = new SettingsPage(client);
                            settingsPage.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Transaction limit update failed. Client not found.", "eBank");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating transaction limit: " + ex.Message, "eBank");
            }
        }

        private void changeAccountActivity(object sender, RoutedEventArgs e)
        {
            int newAccountActivity; 
            if(client.activity == 0) {
                newAccountActivity = 1;
            }
            else {
                newAccountActivity = 0;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to change your account activity?", "eBank", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string updateQuery = "UPDATE clients SET activity = @newAccountActivity WHERE login = @login";

                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@newAccountActivity", newAccountActivity);
                            command.Parameters.AddWithValue("@login", client.login);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                client.activity = newAccountActivity;

                                if (client.activity == 0)
                                {
                                    MessageBox.Show("The account has been set to inactive. You cannot perform any operations.", "eBank");
                                }
                                else if (client.activity == 1)
                                {
                                    MessageBox.Show("The account has been set to active.", "eBank");
                                }

                                SettingsPage settingsPage = new SettingsPage(client);
                                settingsPage.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Account activity update failed. Client not found.", "eBank");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating account activity: " + ex.Message, "eBank");
                }
            }
        }

        private void blockCard(object sender, RoutedEventArgs e)
        {
            int cardActivity = 0;
            string cardNumber = "";
            string cardColor = "";
            string cardStartDate = "";
            string cardEndDate = "";

            MessageBoxResult result = MessageBox.Show("Are you sure you want to block your card?", "eBank", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string updateQuery = "UPDATE clients SET cardActivity = @cardActivity, cardNumber = @cardNumber, cardColor = @cardColor, cardStartDate = @cardStartDate, cardEndDate = @cardEndDate WHERE login = @login";

                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@cardActivity", cardActivity);
                            command.Parameters.AddWithValue("@cardNumber", cardNumber);
                            command.Parameters.AddWithValue("@cardColor", cardColor);
                            command.Parameters.AddWithValue("@cardStartDate", cardStartDate);
                            command.Parameters.AddWithValue("@cardEndDate", cardEndDate);
                            command.Parameters.AddWithValue("@login", client.login);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                client.cardActivity = cardActivity;
                                client.cardNumber = cardNumber;
                                client.cardColor = cardColor;
                                client.cardStartDate = cardStartDate;
                                client.cardEndDate = cardEndDate;
                                MessageBox.Show("The card has been blocked. Create a new card.", "eBank");
                                
                                SettingsPage settingsPage = new SettingsPage(client);
                                settingsPage.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Card activity update failed. Client not found.", "eBank");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error blocking card: " + ex.Message, "eBank");
                }
            }
        }
    }
}
