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
using System.Windows.Navigation;
using System.Windows.Shapes;
using D.Net.EmailClient;
using D.Net.EmailInterfaces;
using System.Data.SQLite;

namespace Email_Listener
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            InitializeComponent();
        } 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void settings_click(object sender, RoutedEventArgs e)
        {
          
        }

        private void clos(object sender, MouseButtonEventArgs e)
        {
            Session.windows_list.Remove(this);
            this.Close();
            Start.refresh_windows();
        }

        private void sett(object sender, MouseButtonEventArgs e)
        {
            Settings window = new Settings();
            window.Show();
        }
    }
}
