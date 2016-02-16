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
using System.Deployment.Application;
using System.Diagnostics;
using System.Reflection;
namespace Email_Listener
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
           // Session.ssl = (bool)ssl.IsChecked;
        }
        private void add_Click(object sender, RoutedEventArgs e)
        {
            Logindata loginwindow = new Logindata();
            loginwindow.Show();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void info_Click(object sender, RoutedEventArgs e)
        {
            string v = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            MessageBox.Show("Email Listener\nŁukasz Cholewa 2015\npglg95@op.pl\n\nv."+v, "Email Listener Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void list_source_refresh()
        {
            maillist.Items.Clear();
            for(int i=0;i<Session.clients_list.Count;i++)
            {
                maillist.Items.Add(Session.clients_list[i].login.ToString());
            }
        }

        private void activated(object sender, EventArgs e)
        {
            list_source_refresh();
            fre.Text = Session.f.ToString();
            port.Text = Session.port.ToString();
            ssl.IsChecked = Session.ssl;
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            Session.delete_client(maillist.SelectedValue.ToString());
            MessageBox.Show("Email address was deleted from the list","Info",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void ssl_changed(object sender, RoutedEventArgs e)
        {
           
        }
        private void okk()
        {
            int sslbase;
            bool help = false;
            try
            {
                if (Session.f != int.Parse(fre.Text)) help = true;
                Session.f = int.Parse(fre.Text);
                Session.port = int.Parse(port.Text);
                Session.ssl = (bool)ssl.IsChecked;
                if ((bool)ssl.IsChecked) sslbase = 1;
                else sslbase = 0;
                SQLBase.give_order("UPDATE Settings SET Ssl=" + sslbase.ToString());
                SQLBase.give_order("UPDATE Settings SET Fre=" + fre.Text);
                SQLBase.give_order("UPDATE Settings SET Port=" + port.Text);
                if (help)
               {
                    Start.restart();
                }
                else 
                this.Close();
            }
            catch
            {
                MessageBox.Show("Enter integer");
            }
        }
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            okk();
        }

      /*  private void start_Click(object sender, RoutedEventArgs e)
        {
            Start.autostart();
        }*/

       /* private void update_Click(object sender, RoutedEventArgs e)
        {
          UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();

                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                      
                        if (MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available",MessageBoxButton.YesNo)==MessageBoxResult.No)
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " +
                            "version to version " + info.MinimumRequiredVersion.ToString() +
                            ". The application will now install the update and restart.",
                            "Update Available", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Process.Start(Application.ResourceAssembly.Location);
                            Application.Current.Shutdown();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
                            return;
                        }
                    }
                }
            }
            Start.update();
        }*/

        private void enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) okk();
        }
    }
}
