using D.Net.EmailClient;
using D.Net.EmailInterfaces;
using Email_Listener.Properties;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using AutoUpdaterDotNET;
using System.Globalization;


namespace Email_Listener
{
   public static class Start
    {
      
       public static string path()
       {
           string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
           if (Environment.OSVersion.Version.Major >= 6)
           {
               path = Directory.GetParent(path).ToString();
           }
           if (!System.IO.Directory.Exists(path+"\\Email Listener"))
           {
               System.IO.Directory.CreateDirectory(path+"\\Email Listener");
           }
           path += "\\Email Listener";
           return path;
       }

        public static void start_dbase()
        {
            try
            {   
                SQLBase.start_connection();
                SQLiteDataReader r = SQLBase.ask("select * from Clients");
                while (r.Read())
                {
                    Session.clients_list.Add(new MClient(EmailClientFactory.GetClient(EmailClientEnum.IMAP), r["Login"].ToString(), r["Password"].ToString(), r["Imap"].ToString()));
                    SQLBase.give_order("UPDATE Clients SET H_number=" + (Session.clients_list.Count - 1).ToString() + " Where Login=" + SQLBase.prepare_to_sql(r["Login"].ToString()));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Problem with database");
            }
        }
       public static void start_dbase2()
       {
           try
           {
               SQLiteDataReader r = SQLBase.ask("select * from Settings");
               while (r.Read())
               {
                   if (int.Parse(r["Ssl"].ToString()) == 0) Session.ssl = false;
                   else if (int.Parse(r["Ssl"].ToString()) == 1) Session.ssl = true;
                   Session.port = int.Parse(r["Port"].ToString());
                   Session.f = int.Parse(r["Fre"].ToString());
               }
           }
           catch (Exception)
           {
               MessageBox.Show("Problem with database");
           }

       }
        public static void start_connecting()
        {
            for (int i = 0; i < Session.clients_list.Count; i++)
            {
                try
                {
                    if(!Session.clients_list[i].client_obj.IsConnected)
                    {
                        Session.clients_list[i].connect(Session.port,Session.ssl);
                        Session.clients_list[i].set_folder("INBOX");
                    }
                }
                catch
                {
                   // MessageBox.Show("No network");
               }
            }
        }
       public static void reconnecting()
        {
           for(int i=0;i<Session.clients_list.Count;i++)
           {
               try
               {
                   if(Session.clients_list[i].client_obj.IsConnected)
                   {
                       Session.clients_list[i].disconnect();
                   }
                   Session.clients_list[i].connect(Session.port, Session.ssl);
                   Session.clients_list[i].set_folder("INBOX");
               }
               catch
               {
                   //no network
               }
           }
        }
        public static void show(string mailto, string mailfrom, string subject) //call by check_emails()
        {

            MainWindow win = new MainWindow();
            win.textbox.Text =subject+" | "+ mailto+" <== "+ mailfrom;
            Session.windows_list.Add(win);
            int count = Session.windows_list.Count;
            win.Left =System.Windows.SystemParameters.WorkArea.Width - win.Width;
            win.Top = System.Windows.SystemParameters.WorkArea.Height - win.Height*count;
            win.Show();
        }
        public static List<string> check_emails()
        {
            SQLiteDataReader r;
            int sq=-1;
            bool first = true;
            List<string> re = new List<string>();
            re.Clear();
            for (int i = 0; i < Session.clients_list.Count; i++) 
            {
                Session.clients_list[i].client_obj.Messages.Clear();
                r = SQLBase.ask("select * from Clients");
                while(r.Read())
                {
                    if(r["Login"].ToString()==Session.clients_list[i].login)
                    {
                        sq = int.Parse(r["Sqnumber"].ToString());
                        break;
                    }
                }
                Start.reconnecting();
                if (sq == -1) Session.clients_list[i].load_emails();
                else
                {
                    Session.clients_list[i].load_emails(sq);

                }
                first = true; 
                for (int j = Session.clients_list[i].client_obj.Messages.Count-1; j >=0; j--)
                {
                    if(sq==-1)
                    {
                        IEmail msm = (IEmail)Session.clients_list[i].client_obj.Messages[j];
                        msm.LoadInfos();
                        SQLBase.give_order("UPDATE Clients SET Sqnumber=" + msm.SequenceNumber.ToString() + " Where Login=" + SQLBase.prepare_to_sql(Session.clients_list[i].login));
                        break;

                    }
                    else
                    {
                        IEmail msm = (IEmail)Session.clients_list[i].client_obj.Messages[j];
                        msm.LoadInfos();
                        if(first)
                        {
                            SQLBase.give_order("UPDATE Clients SET Sqnumber=" + msm.SequenceNumber.ToString() + " Where Login=" + SQLBase.prepare_to_sql(Session.clients_list[i].login));
                            first = false;
                        }
                        re.Add( msm.From[0]);
                        re.Add( msm.Subject);
                        re.Add ( Session.clients_list[i].login);
                    }
                }
            }
            return re;
        }
       public static void refresh_windows()
        {
           for(int i=0;i<Session.windows_list.Count;i++)
           {
               Session.windows_list[i].Visibility = Visibility.Collapsed;
               Session.windows_list[i].Top =System.Windows.SystemParameters.WorkArea.Height - Session.windows_list[i].Height * (i+1);
               Session.windows_list[i].Visibility = Visibility.Visible;
           }
        }
       public static void wait()
       {
           DateTime now = DateTime.Now;
           TimeSpan b = new TimeSpan(00, 01, 00);
           for (; ; )
           {
               TimeSpan a = DateTime.Now - now;
               if (a >= b)
               {
                   now = DateTime.Now;
               }
           }
       }
       public static void first_start()
       {
           string path2 = Start.path() + "\\f.ch";
           if (!File.Exists(path2))
           {
               File.Create(path2);
               Startpage w = new Startpage();
               w.Show();
           }
       }
       public static void autostart()
       {
           try
           {
               RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
               registryKey.SetValue("Email Listener", Assembly.GetExecutingAssembly());
           }
           catch
           {
               MessageBox.Show("Run program as Admin to add it to autostart", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
           }
       }
       public static void shutdown()
       {
           Application.Current.Shutdown();
       }
       public static void update()
       {
          /* try
           {
                AutoUpdater.Start("");
           }
           catch
           {

           }
           //AutoUpdater.CurrentCulture = CultureInfo.CreateSpecificCulture("pl");*/
           
       }
       public static void restart()
       {
           Application.Current.Exit += delegate(object sender, ExitEventArgs e)
           {
               System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
           };
           Application.Current.Shutdown();
       }
    }
}
