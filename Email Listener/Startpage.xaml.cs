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

namespace Email_Listener
{
    /// <summary>
    /// Interaction logic for Startpage.xaml
    /// </summary>
    public partial class Startpage : Window
    {
        public Startpage()
        {
            InitializeComponent();
        }

        private void startkey_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Settings a = new Settings();
            Logindata b = new Logindata();
            a.Show();
            b.Show();
        }
    }
}
