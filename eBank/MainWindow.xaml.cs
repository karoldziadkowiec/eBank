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
        string connectionString = "Server=.;Database=eBank;Integrated Security=True;";
        public MainWindow()
        {
            InitializeComponent();

            accountType_ComboBox.Items.Add("Admin");
            accountType_ComboBox.Items.Add("Client");

            accountType_ComboBox.SelectedItem = "Client";
        }

        private void goToHomePage(object sender, RoutedEventArgs e)
        {
            string login = login_TextBox.Text;
            string password = password_PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Complete the empty fields.", "eBank");
                return;
            }

            MainWindow homePage = new MainWindow();
            homePage.Show();
            this.Hide();

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
