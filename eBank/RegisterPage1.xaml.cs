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
    /// Logika interakcji dla klasy RegisterPage1.xaml
    /// </summary>
    public partial class RegisterPage1 : Window
    {
        public RegisterPage1()
        {
            InitializeComponent();

            accountType_ComboBox.Items.Add("Admin");
            accountType_ComboBox.Items.Add("Client");

            accountType_ComboBox.SelectedItem = "Client";
        }

        private void goToNextRegisterPage(object sender, RoutedEventArgs e)
        {
            RegisterPage2 registerPage2 = new RegisterPage2();
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
