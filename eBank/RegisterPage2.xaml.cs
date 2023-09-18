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
        public RegisterPage2()
        {
            InitializeComponent();
        }

        private void goToNextRegisterPage(object sender, RoutedEventArgs e)
        {
            RegisterPage3 registerPage3 = new RegisterPage3();
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
