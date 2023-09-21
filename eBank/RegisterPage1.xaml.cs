using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
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
    /// Logika interakcji dla klasy RegisterPage1.xaml
    /// </summary>
    public partial class RegisterPage1 : Window
    {
        Client client = null;

        public RegisterPage1()
        {
            InitializeComponent();
            displayData();
        }

        private void displayData()
        {
            accountType_ComboBox.Items.Add("Admin");
            accountType_ComboBox.Items.Add("Client");

            accountType_ComboBox.SelectedItem = "Client";
        }

        private void goToNextRegisterPage(object sender, RoutedEventArgs e)
        {
            string accountType = accountType_ComboBox.Text;
            string name = name_TextBox.Text;
            string surname = surname_TextBox.Text;
            string peselNumber = peselNumber_TextBox.Text;
            string idCardNumber = idCardNumber_TextBox.Text;
            string birthday = birthday_DatePicker.Text;
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

            string creationDate = DateTime.Today.ToString("yyyy-MM-dd");
            int activity = 1;
            double checkingAccount = 0.0d;
            double savingsAccount = 0.0d;
            double withdrawalLimit = 1000.0d;
            double transactionLimit = 1000.0d;
            int cardActivity = 0;
            string cardNumber = "";
            string cardColor = "";
            string cardStartDate = "";
            string cardEndDate = "";


            if (string.IsNullOrEmpty(accountType) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(peselNumber) || string.IsNullOrEmpty(idCardNumber) || string.IsNullOrEmpty(birthday) || string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Complete the empty fields.", "eBank");
                return;
            }
            if (peselNumber.Length != 11)
            {
                MessageBox.Show("The PESEL number must contain 11 characters.", "eBank");
                return;
            }
            if (idCardNumber.Length != 9)
            {
                MessageBox.Show("The ID card number must contain 9 characters.", "eBank");
                return;
            }

            client = new Client(
                ID: 0,
                ACCOUNTTYPE: accountType,
                PESELNUMBER: peselNumber,
                NAME: name,
                SURNAME: surname,
                LOGIN: "",
                PASSWORD: "",
                CHECKINGACCOUNT: checkingAccount,
                SAVINGSACCOUNT: savingsAccount,
                ACTIVITY: activity,
                GENDER: gender,
                BIRTHDAY: birthday,
                IDCARDNUMBER: idCardNumber,
                PLACEOFBIRTH: "",
                RESIDENTIALADDRESS: "",
                CORRESPONDENCEADDRESS: "",
                PHONENUMBER: "",
                EMAIL: "",
                PASSWORDREMINDER: "",
                WITHDRAWALLIMIT: withdrawalLimit,
                TRANSACTIONLIMIT: transactionLimit,
                CREATIONDATE: creationDate,
                CARDNUMBER: cardNumber,
                CARDACTIVITY: cardActivity,
                CARDCOLOR: cardColor,
                CARDSTARTDATE: cardStartDate,
                CARDENDDATE: cardEndDate
            );

            RegisterPage2 registerPage2 = new RegisterPage2(client);
            registerPage2.Show();
            this.Hide();
        }

        private void goToLoginPage(object sender, RoutedEventArgs e)
        {
            MainWindow loginPage = new MainWindow();
            loginPage.Show();
            this.Hide();
        }

    }
}
