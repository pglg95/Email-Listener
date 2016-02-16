using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using System.ComponentModel;
using System.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using System.Drawing;
using System.Windows.Controls.Primitives;


namespace Email_Listener
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon tb;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private void Application_Startup(object sender, StartupEventArgs e)
        {


            Start.first_start();
            tb = (TaskbarIcon) FindResource("MyN");

         
            Start.update();
            Start.start_dbase();
            Start.start_dbase2();
   
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.RunWorkerAsync();

        }


        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //MessageBox.Show("progress changed");
            List <string> a=(List<string>)e.UserState;
            for(int i=0;i<a.Count;i+=3)
            {
                Start.show(a[i+2],a[i],a[i+1]);
            }
        }


        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int a = 1;
            for(;;)
            {
                //MessageBox.Show("do work");
                Start.start_connecting();
               backgroundWorker.ReportProgress(a,Start.check_emails());
                a++;
                Thread.Sleep(Session.f*60*1000);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Start.shutdown();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Settings a = new Settings();
            a.Show();
        }
    }
}
