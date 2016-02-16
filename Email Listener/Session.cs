using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Threading;
using D.Net.EmailInterfaces;
using D.Net.EmailClient;
using System.Collections;
using System.Data.SQLite;
using System.Threading;

namespace Email_Listener
{
    public static class Session
    {
        public static List<MClient> clients_list = new List<MClient>();
        public static bool ssl { get; set; }
        public static int f { get; set; }
        public static int port { get; set; }
        public static List<MainWindow> windows_list = new List<MainWindow>();
        private static MClient check_client(string login, string password, string imap, bool ssl, int port)
        {
            IEmailClient org=EmailClientFactory.GetClient(EmailClientEnum.IMAP);
            MClient temp=new MClient(org,login,password,imap);
            temp.connect(port, ssl);
            temp.set_folder("INBOX");
            return temp;
        }
        public static bool initialize_new_client(string login, string password, string imap)
        {
            try
            {
                clients_list.Add(check_client(login, password, imap, ssl,port));
                SQLBase.give_order("insert into Clients (Login,Password,Imap,H_number,Sqnumber) values (" + SQLBase.prepare_to_sql(login) + "," + SQLBase.prepare_to_sql(password) + "," + SQLBase.prepare_to_sql(imap) + ","+(Session.clients_list.Count()-1).ToString()+","+ (-1).ToString()+      ")");
                return true;
            }
            catch (EMailException)
            {
                return false;
            }
        }
        public static void delete_client(string login)
        {
            SQLiteDataReader r = SQLBase.ask("select * from Clients");
            bool find = false;
            while(r.Read())
            {
                if(r["Login"].ToString()==login)
                {
                    clients_list.RemoveAt(int.Parse(r["H_number"].ToString()));
                    find = true;
                }
                else if(find)
                {
                    SQLBase.give_order("UPDATE Clients SET H_number=" + (int.Parse(r["H_number"].ToString()) - 1).ToString() + " Where Login=" + SQLBase.prepare_to_sql(r["Login"].ToString()));
                }
            }
            SQLBase.give_order("DELETE FROM Clients WHERE Login=" + SQLBase.prepare_to_sql(login));
        }
    }
}

