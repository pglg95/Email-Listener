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
using D.Net.EmailClient;
using D.Net.EmailInterfaces;
using System.ComponentModel;
using System.Threading;

namespace Email_Listener
{
    /// <summary>
    /// Interaction logic for Logindata.xaml
    /// </summary>
    public partial class Logindata : Window
    {
        private BackgroundWorker backgroundWorker= new BackgroundWorker();
        Load okno3 = new Load();
        private bool stopclosing = false;
        public Logindata()
        {
            InitializeComponent();

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.WorkerReportsProgress = true;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if((bool)e.UserState)
            {
                MessageBox.Show("Your email address was added to the list\nFor each new message, notification will be displayed!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong login,password or IMAP. Maybe you do not have network access", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                clear();
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            chvis(true);
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> a = (List<string>)e.Argument;
            bool result = Session.initialize_new_client(a[0], a[1], a[2]);
            backgroundWorker.ReportProgress(100,result);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }
        private void clear()
        {
            password.Password= null;
        }
        private void ok()
        {
            chvis(false);
           List<string> list_of_args = new List<string>();
            list_of_args.Add(login.Text);
            list_of_args.Add(password.Password);
            list_of_args.Add(imap.Text);
            backgroundWorker.RunWorkerAsync(list_of_args);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           ok();
        }
        private void chvis(bool a)
        {
            if(!a)
            {
                grup.Opacity = 0.5;
                login.IsEnabled = false;
                password.IsEnabled = false;
                imap.IsEnabled = false;
                pasek.Visibility = Visibility.Visible;
                czekaj.Visibility = Visibility.Visible;
                yes.IsEnabled = false;
                anuluj.IsEnabled = false;
                stopclosing = true;
            }
            else
            {
                grup.Opacity = 1;
                login.IsEnabled = true;
                password.IsEnabled = true;
                imap.IsEnabled = true;
                pasek.Visibility = Visibility.Hidden;
                czekaj.Visibility = Visibility.Hidden;
                yes.IsEnabled = true;
                anuluj.IsEnabled = true;
                stopclosing = false;
            }
        }
        private void enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) ok();
        }
        private void delete_client()
        {

        }

        private void closing(object sender, CancelEventArgs e)
        {
            if(stopclosing)
            e.Cancel = true;
        }


        
    }
}
