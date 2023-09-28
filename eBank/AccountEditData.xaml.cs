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
    /// Logika interakcji dla klasy AccountEditData.xaml
    /// </summary>
    public partial class AccountEditData : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client = null;
        public AccountEditData(Client _client)
        {
            client = _client;
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
            displayAccountData();
        }

        private void displayAccountData()
        {
            name_TextBox.Text = client.name;
            surname_TextBox.Text = client.surname;
            idCardNumber_TextBox.Text = client.idCardNumber;
            login_TextBox.Text = client.login;
            password_PasswordBox.Password = client.password;
            confirmPassword_PasswordBox.Password = client.password;
            if (client.gender == "Man")
            {
                man_RadioButton.IsChecked = true;
            }
            else if (client.gender == "Woman")
            {
                woman_RadioButton.IsChecked = true;
            }
            else if (client.gender == "Other")
            {
                other_RadioButton.IsChecked = true;
            }

            birthday_DatePicker.Text = client.birthday;
            telephoneNumber_TextBox.Text = client.phoneNumber;
            email_TextBox.Text = client.email;
            placeOfBirth_TextBox.Text = client.placeOfBirth;
            residentialAddress_TextBox.Text = client.residentialAddress;
            correspondencyAddress_TextBox.Text = client.correspondencyAddress;
            passwordReminder_TextBox.Text = client.passwordReminder;
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

        private void goToOrderCardPage(object sender, RoutedEventArgs e)
        {
            ServicesOrderCard servicesOrderCard = new ServicesOrderCard(client);
            servicesOrderCard.Show();
            this.Hide();
        }

        private void saveAccountData(object sender, RoutedEventArgs e)
        {
            string name = name_TextBox.Text;
            string surname = surname_TextBox.Text;
            string idCardNumber = idCardNumber_TextBox.Text;
            string login = login_TextBox.Text;
            string password = password_PasswordBox.Password;
            string confirmPassword = confirmPassword_PasswordBox.Password;
            string gender = "";

            if (man_RadioButton.IsChecked == true)
            {
                gender = "Man";
            }
            else if (woman_RadioButton.IsChecked == true)
            {
                gender = "Woman";
            }
            else if (other_RadioButton.IsChecked == true)
            {
                gender = "Other";
            }
            else
            {
                MessageBox.Show("Select a gender.", "eBank");
                return;
            }

            string birthday = birthday_DatePicker.Text;
            string telephoneNumber = telephoneNumber_TextBox.Text;
            string email = email_TextBox.Text;
            string placeOfBirth = placeOfBirth_TextBox.Text;
            string residentialAddress = residentialAddress_TextBox.Text;
            string correspondencyAddress = correspondencyAddress_TextBox.Text;
            string passwordReminder = passwordReminder_TextBox.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(idCardNumber) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(birthday) || string.IsNullOrEmpty(telephoneNumber) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(placeOfBirth) || string.IsNullOrEmpty(residentialAddress) || string.IsNullOrEmpty(correspondencyAddress) || string.IsNullOrEmpty(passwordReminder))
            {
                MessageBox.Show("Complete all required fields.", "eBank");
                return;
            }

            if (idCardNumber.Length != 9)
            {
                MessageBox.Show("The ID card number must contain 9 characters.", "eBank");
                return;
            }

            if (login.Length < 5 || login.Length > 20)
            {
                MessageBox.Show("The login must contain from 5 to 20 characters.", "eBank");
                return;
            }

            if (password.Length < 5 || password.Length > 30)
            {
                MessageBox.Show("The password must contain between 5 and 30 characters.", "eBank");
                return;
            }

            if (!password.Equals(confirmPassword))
            {
                MessageBox.Show("Passwords do not match.", "eBank");
                return;
            }

            if (telephoneNumber.Length != 9)
            {
                MessageBox.Show("The phone number must contain 9 characters.", "eBank");
                return;
            }
            if (!email.Contains('@') || !email.Contains('.'))
            {
                MessageBox.Show("The email address must contain the characters: '@' and '.'", "eBank");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE clients SET name=@name, surname=@surname, idCardNumber=@idCardNumber, login=@login, password=@password, gender=@gender, birthday=@birthday, telNumber=@telephoneNumber, email=@email, placeOfBirth=@placeOfBirth, residentialAddress=@residentialAddress, correspondenceAddress=@correspondencyAddress, passwordReminder=@passwordReminder WHERE id=@clientID";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@surname", surname);
                        command.Parameters.AddWithValue("@idCardNumber", idCardNumber);
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@gender", gender);
                        command.Parameters.AddWithValue("@birthday", birthday);
                        command.Parameters.AddWithValue("@telephoneNumber", telephoneNumber);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@placeOfBirth", placeOfBirth);
                        command.Parameters.AddWithValue("@residentialAddress", residentialAddress);
                        command.Parameters.AddWithValue("@correspondencyAddress", correspondencyAddress);
                        command.Parameters.AddWithValue("@passwordReminder", passwordReminder);
                        command.Parameters.AddWithValue("@clientID", client.id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            client.name = name;
                            client.surname = surname;
                            client.idCardNumber = idCardNumber;
                            client.login = login;
                            client.password = password;
                            client.gender = gender;
                            client.birthday = birthday;
                            client.phoneNumber = telephoneNumber;
                            client.email = email;
                            client.placeOfBirth = placeOfBirth;
                            client.residentialAddress = residentialAddress;
                            client.correspondencyAddress = correspondencyAddress;
                            client.passwordReminder = passwordReminder;

                            MessageBox.Show("Account data successfully updated.", "eBank");

                            AccountPage accountPage = new AccountPage(client);
                            accountPage.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Account data update failed. Please correct the data.", "eBank");
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "eBank");
            }
        }

        private void backToAccountPage(object sender, RoutedEventArgs e)
        {
            AccountPage accountPage = new AccountPage(client);
            accountPage.Show();
            this.Hide();
        }
    }
}
