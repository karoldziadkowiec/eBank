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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eBank
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client = null;
        public MainWindow()
        {
            InitializeComponent();
            displayData();
            clickEnter();
        }
        
        private void displayData()
        {
            accountType_ComboBox.Items.Add("Admin");
            accountType_ComboBox.Items.Add("Client");

            accountType_ComboBox.SelectedItem = "Client";
        }

        private void clickEnter()
        {
            password_PasswordBox.KeyUp += PasswordBox_EnterKeyPressed;
        }

        private void PasswordBox_EnterKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                goToHomePage(null, null);
            }
        }

        private void goToHomePage(object sender, RoutedEventArgs e)
        {
            string accountType = accountType_ComboBox.Text;
            string login = login_TextBox.Text;
            string password = password_PasswordBox.Password;

            if (string.IsNullOrEmpty(accountType))
            {
                MessageBox.Show("Select account type.", "eBank");
                return;
            }

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Complete the empty login fields.", "eBank");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();

                try
                {
                    connection.Open();

                    string query = "SELECT * FROM clients WHERE accountType = @accountType AND login = @login AND password = @password";
                    command.CommandText = query;
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@accountType", accountType);
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                accountType = reader.GetString(1);
                                string peselNumber = reader.GetString(2);
                                string name = reader.GetString(3);
                                string surname = reader.GetString(4);
                                login = reader.GetString(5);
                                password = reader.GetString(6);
                                double checkingAccount = reader.GetDouble(7);
                                double savingsAccount = reader.GetDouble(8);
                                int activity = reader.GetInt32(9);
                                string gender = reader.GetString(10);
                                DateTime birthday = reader.GetDateTime(11);
                                string birthdayString = birthday.ToString("yyyy-MM-dd");
                                string idCardNumber = reader.GetString(12);
                                string placeOfBirth = reader.GetString(13);
                                string residentialAddress = reader.GetString(14);
                                string correspondencyAddress = reader.GetString(15);
                                string phoneNumber = reader.GetString(16);
                                string email = reader.GetString(17);
                                string passwordReminder = reader.GetString(18);
                                double withdrawalLimit = reader.GetDouble(19);
                                double transactionLimit = reader.GetDouble(20);
                                DateTime creationDate = reader.GetDateTime(21);
                                string creationDateString = creationDate.ToString("yyyy-MM-dd");
                                string cardNumber = reader.GetString(22);
                                int cardActivity = reader.GetInt32(23);
                                string cardColor = reader.GetString(24);
                                DateTime cardStartDate = reader.GetDateTime(25);
                                string ccardStartDateString = cardStartDate.ToString("yyyy-MM-dd");
                                DateTime cardEndDate = reader.GetDateTime(26);
                                string cardEndDateString = cardEndDate.ToString("yyyy-MM-dd");

                                client = new Client(id, accountType, peselNumber, name, surname, login, password, 
                                    checkingAccount, savingsAccount, activity, gender, birthdayString, idCardNumber, 
                                    placeOfBirth, residentialAddress, correspondencyAddress, phoneNumber, email, 
                                    passwordReminder, withdrawalLimit, transactionLimit, creationDateString, 
                                    cardNumber, cardActivity, cardColor, ccardStartDateString, cardEndDateString);

                                if (client.accountType == "Client") {
                                    HomePage homePage = new HomePage(client);
                                    homePage.Show();
                                    this.Hide();
                                }
                                else {
                                    AdminHomePage adminHomePage = new AdminHomePage(client);
                                    adminHomePage.Show();
                                    this.Hide();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please, enter the correct login details.", "eBank");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "eBank");
                }
            }
        }

        private void goToRegisterPage(object sender, RoutedEventArgs e)
        {
            RegisterPage1 registerPage1 = new RegisterPage1();
            registerPage1.Show();
            this.Hide();
        }

        private void goToPasswordReminderPage(object sender, RoutedEventArgs e)
        {
            PasswordReminderPage passwordReminderPage = new PasswordReminderPage();
            passwordReminderPage.Show();
            this.Hide();
        }
    }
}
