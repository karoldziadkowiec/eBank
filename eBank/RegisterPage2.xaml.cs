using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy RegisterPage2.xaml
    /// </summary>
    public partial class RegisterPage2 : Window
    {
        Client client = null;
        public RegisterPage2(Client _client)
        {
            client = _client;
            InitializeComponent();
        }

        private void goToNextRegisterPage(object sender, RoutedEventArgs e)
        {
            string email = email_TextBox.Text;
            string phoneNumber = phoneNumber_TextBox.Text;
            string placeOfBirth = placeOfBirth_TextBox.Text;
            string residentialAddress = residentialAddress_TextBox.Text;
            string correspondenceAddress = correspondenceAddress_TextBox.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(placeOfBirth) || string.IsNullOrEmpty(residentialAddress) || string.IsNullOrEmpty(correspondenceAddress))
            {
                MessageBox.Show("Complete the empty fields.", "eBank");
                return;
            }
            if (phoneNumber.Length != 9)
            {
                MessageBox.Show("The phone number must contain 9 characters.", "eBank");
                return;
            }
            if (!email.Contains('@') || !email.Contains('.'))
            {
                MessageBox.Show("The email address must contain the characters: '@' and '.'", "eBank");
                return;
            }

            client.email = email;
            client.phoneNumber = phoneNumber;
            client.placeOfBirth = placeOfBirth;
            client.residentialAddress = residentialAddress;
            client.correspondenceAddress = correspondenceAddress;

            RegisterPage3 registerPage3 = new RegisterPage3(client);
            registerPage3.Show();
            this.Hide();
        }

        private void goToPreviousRegisterPage(object sender, RoutedEventArgs e)
        {
            RegisterPage1 registerPage1 = new RegisterPage1();
            registerPage1.Show();
            this.Hide();
        }

    }
}
