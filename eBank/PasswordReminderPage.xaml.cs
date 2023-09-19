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
    /// Logika interakcji dla klasy PasswordReminderPage.xaml
    /// </summary>
    public partial class PasswordReminderPage : Window
    {
        private readonly string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        Client client = null;
        public PasswordReminderPage()
        {
            InitializeComponent();
            InitializeStartVisibility();
        }

        private void InitializeStartVisibility()
        {
            passwordReminder_Label.Visibility = Visibility.Hidden;
            r1_Rectangle.Visibility = Visibility.Hidden;
            question_Label.Visibility = Visibility.Hidden;
            passwordReminder_TextBox.Visibility = Visibility.Hidden;
            next2_Button.Visibility = Visibility.Hidden;
            newPassword_Label.Visibility = Visibility.Hidden;
            r2_Rectangle.Visibility = Visibility.Hidden;
            newPassword_PasswordBox.Visibility = Visibility.Hidden;
            confirmPassword_Label.Visibility = Visibility.Hidden;
            r3_Rectangle.Visibility = Visibility.Hidden;
            confirmPassword_PasswordBox.Visibility = Visibility.Hidden;
            acceptance_CheckBox.Visibility = Visibility.Hidden;
            save_Button.Visibility = Visibility.Hidden;
        }

        private void showPasswordReminderTextBox(object sender, RoutedEventArgs e)
        {
            string login = login_TextBox.Text;

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Complete the empty login field.", "eBank");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();

                try
                {
                    connection.Open();

                    string query = "SELECT * FROM clients WHERE login = @login";
                    command.CommandText = query;
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@login", login);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string accountType = reader.GetString(1);
                                string peselNumber = reader.GetString(2);
                                string name = reader.GetString(3);
                                string surname = reader.GetString(4);
                                login = reader.GetString(5);
                                string password = reader.GetString(6);
                                double checkingAccount = reader.GetDouble(7);
                                double savingsAccount = reader.GetDouble(8);
                                int activity = reader.GetInt32(9);
                                string gender = reader.GetString(10);
                                DateTime birthday = reader.GetDateTime(11);
                                string birthdayString = birthday.ToString("yyyy-MM-dd");
                                string idCardNumber = reader.GetString(12);
                                string placeOfBirth = reader.GetString(13);
                                string residentialAddress = reader.GetString(14);
                                string correspondenceAddress = reader.GetString(15);
                                string email = reader.GetString(16);
                                string phoneNumber = reader.GetString(17);
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
                                    placeOfBirth, residentialAddress, correspondenceAddress, email, phoneNumber,
                                    passwordReminder, withdrawalLimit, transactionLimit, creationDateString,
                                    cardNumber, cardActivity, cardColor, ccardStartDateString, cardEndDateString);

                                login_TextBox.IsReadOnly = true;
                                next1_Button.Visibility = Visibility.Hidden;
                                passwordReminder_Label.Visibility = Visibility.Visible;
                                r1_Rectangle.Visibility = Visibility.Visible;
                                question_Label.Visibility = Visibility.Visible;
                                passwordReminder_TextBox.Visibility = Visibility.Visible;
                                next2_Button.Visibility = Visibility.Visible;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please, enter the correct login.", "eBank");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "eBank");
                }
            }
        }

        private void showPasswordTextBoxes(object sender, RoutedEventArgs e)
        {
            string passwordReminder = passwordReminder_TextBox.Text;

            if (client.passwordReminder == passwordReminder)
            {
                passwordReminder_TextBox.IsReadOnly = true;
                next2_Button.Visibility = Visibility.Hidden;
                newPassword_Label.Visibility = Visibility.Visible;
                r2_Rectangle.Visibility = Visibility.Visible;
                newPassword_PasswordBox.Visibility = Visibility.Visible;
                confirmPassword_Label.Visibility = Visibility.Visible;
                r3_Rectangle.Visibility = Visibility.Visible;
                confirmPassword_PasswordBox.Visibility = Visibility.Visible;
                acceptance_CheckBox.Visibility = Visibility.Visible;
                save_Button.Visibility = Visibility.Visible;
            }
            else if (string.IsNullOrEmpty(passwordReminder)) {
                MessageBox.Show("Complete the password reminder field.", "eBank");
                return;
            }
            else
            {
                MessageBox.Show("Please, enter the correct answer.", "eBank");
            }
        }

        private void goToLoginPage(object sender, RoutedEventArgs e)
        {
            string newPassword = newPassword_PasswordBox.Password;
            string confirmPassword = confirmPassword_PasswordBox.Password;

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Complete the empty password fields.", "eBank");
                return;
            }
            else if (newPassword.Length < 5 || newPassword.Length > 30)
            {
                MessageBox.Show("The password must contain between 5 and 30 characters.", "eBank");
                return;
            }
            else if (string.Compare(newPassword, confirmPassword) != 0)
            {
                MessageBox.Show("Passwords do not match.", "eBank");
                return;
            }

            else if (!acceptance_CheckBox.IsChecked.HasValue || !acceptance_CheckBox.IsChecked.Value)
            {
                MessageBox.Show("Accept the terms of the bank.", "eBank");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string updateQuery = "UPDATE clients SET password = @newPassword WHERE login = @login";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@newPassword", newPassword);
                        command.Parameters.AddWithValue("@login", client.login);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password updated successfully.", "eBank");

                            MainWindow loginPage = new MainWindow();
                            loginPage.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Password update failed. User not found.", "eBank");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating password: " + ex.Message, "eBank");
            }
        }

        private void backToLoginPage(object sender, RoutedEventArgs e)
        {
            MainWindow loginPage = new MainWindow();
            loginPage.Show();
            this.Hide();
        }
    }
}
