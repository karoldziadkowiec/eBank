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
using System.Xml.Linq;

namespace eBank
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterPage3.xaml
    /// </summary>
    public partial class RegisterPage3 : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client = null;
        public RegisterPage3(Client _client)
        {
            client = _client;
            InitializeComponent();
        }

        private void goToLoginPage(object sender, RoutedEventArgs e)
        {
            string login = login_TextBox.Text;
            string password = password_PasswordBox.Password;
            string confirmPassword = confirmPassword_PasswordBox.Password;
            string passwordReminder = passwordReminder_TextBox.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(passwordReminder))
            {
                MessageBox.Show("Complete the empty fields.", "eBank");
                return;
            }

            else if (login.Length < 5 || login.Length > 20)
            {
                MessageBox.Show("The login must contain from 5 to 20 characters.", "eBank");
                return;
            }

            else if (password.Length < 5 || password.Length > 30)
            {
                MessageBox.Show("The password must contain between 5 and 30 characters.", "eBank");
                return;
            }
            else if (string.Compare(password, confirmPassword) != 0)
            {
                MessageBox.Show("Passwords do not match.", "eBank");
                return;
            }

            else if (!acceptanceOfTerms_CheckBox.IsChecked.HasValue || !acceptanceOfTerms_CheckBox.IsChecked.Value)
            {
                MessageBox.Show("Accept the terms of the bank.", "eBank");
                return;
            }

            client.login = login;
            client.password = password;
            client.passwordReminder = passwordReminder;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO clients (accountType, peselNumber, name, surname, login, password, checkingAccount, savingsAccount, activity, gender, birthday, idCardNumber, placeOfBirth, residentialAddress, correspondenceAddress, telNumber, email, passwordReminder, withdrawalLimit, transactionLimit, creationDate, cardNumber, cardActivity, cardColor, cardStartDate, cardEndDate) VALUES (@accountType, @peselNumber, @name, @surname, @login, @password, @checkingAccount, @savingsAccount, @activity, @gender, @birthday, @idCardNumber, @placeOfBirth, @residentialAddress, @correspondenceAddress, @telNumber, @email, @passwordReminder, @withdrawalLimit, @transactionLimit, @creationDate, @cardNumber, @cardActivity, @cardColor, @cardStartDate, @cardEndDate);";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@accountType", client.accountType);
                        command.Parameters.AddWithValue("@peselNumber", client.peselNumber);
                        command.Parameters.AddWithValue("@name", client.name);
                        command.Parameters.AddWithValue("@surname", client.surname);
                        command.Parameters.AddWithValue("@login", client.login);
                        command.Parameters.AddWithValue("@password", client.password);
                        command.Parameters.AddWithValue("@checkingAccount", client.checkingAccount);
                        command.Parameters.AddWithValue("@savingsAccount", client.savingsAccount);
                        command.Parameters.AddWithValue("@activity", client.activity);
                        command.Parameters.AddWithValue("@gender", client.gender);
                        command.Parameters.AddWithValue("@birthday", client.birthday);
                        command.Parameters.AddWithValue("@idCardNumber", client.idCardNumber);
                        command.Parameters.AddWithValue("@placeOfBirth", client.placeOfBirth);
                        command.Parameters.AddWithValue("@residentialAddress", client.residentialAddress);
                        command.Parameters.AddWithValue("@correspondenceAddress", client.correspondenceAddress);
                        command.Parameters.AddWithValue("@telNumber", client.phoneNumber);
                        command.Parameters.AddWithValue("@email", client.email);
                        command.Parameters.AddWithValue("@passwordReminder", client.passwordReminder);
                        command.Parameters.AddWithValue("@withdrawalLimit", client.withdrawalLimit);
                        command.Parameters.AddWithValue("@transactionLimit", client.transactionLimit);
                        command.Parameters.AddWithValue("@creationDate", client.creationDate);
                        command.Parameters.AddWithValue("@cardNumber", client.cardNumber);
                        command.Parameters.AddWithValue("@cardActivity", client.cardActivity);
                        command.Parameters.AddWithValue("@cardColor", client.cardColor);
                        command.Parameters.AddWithValue("@cardStartDate", client.cardStartDate);
                        command.Parameters.AddWithValue("@cardEndDate", client.cardEndDate);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Account successfully registered.", "eBank");
                    connection.Close();

                    MainWindow loginPage = new MainWindow();
                    loginPage.Show();
                    Close();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Error number for duplicate (unique constraint violation)
                {
                    MessageBox.Show("The given client already has an account. Check your data again: PESEL number, ID card number, login.", "eBank");
                }
                else
                {
                    MessageBox.Show("An error occurred during registration: " + ex.Message, "eBank");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred during registration: " + ex.Message, "eBank");
            }
        }

        private void goToPreviousRegisterPage(object sender, RoutedEventArgs e)
        {
            RegisterPage2 registerPage2 = new RegisterPage2(client);
            registerPage2.Show();
            this.Hide();
        }
    }
}
