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
    /// Logika interakcji dla klasy AdminHomePage.xaml
    /// </summary>
    public partial class AdminHomePage : Window
    {
        Client client = null;
        public AdminHomePage(Client _client)
        {
            client = _client;
            InitializeComponent();
        }
    }
}
