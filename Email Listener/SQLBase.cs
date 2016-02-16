using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D.Net.EmailClient;
using D.Net.EmailInterfaces;
using System.Data.SQLite;
using System.IO;

namespace Email_Listener
{
    static class SQLBase
    {
        public static  SQLiteConnection dbase;
        public static void start_connection()
        {
            string path2 = Start.path() + "\\data.s3db";
            if (!File.Exists(path2))
            {
                SQLiteConnection.CreateFile(path2);
                dbase = new SQLiteConnection("Data Source=" + path2);
                dbase.Open();
                SQLBase.give_order("CREATE TABLE [Clients] ([Id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,[Login] TEXT  UNIQUE NOT NULL,[Password] TEXT  NOT NULL,[Imap] TEXT  NOT NULL,[H_number] INTEGER  NULL,[Sqnumber] INTEGER  NULL)");

                Session.ssl = true;
                Session.port = 993;
                Session.f = 10;
                SQLBase.give_order("CREATE TABLE [Settings] ([Id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,[Ssl] INTEGER NOT NULL,[Fre] INTEGER NOT NULL,[Port] INTEGER NOT NULL)");
                SQLBase.give_order("insert into Settings (Ssl,Fre,Port) values (1,10,993)");
            }
            else
            {
                dbase = new SQLiteConnection("Data Source=" + path2);
                dbase.Open();
            }
        }

        public static void stop_connection()
        {
            dbase.Close();
        }
        public static void give_order(string sql)
        {
             SQLiteCommand command = new SQLiteCommand(sql, dbase);
             command.ExecuteNonQuery();
        }
        public static SQLiteDataReader ask(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, dbase);
            SQLiteDataReader reader = command.ExecuteReader();
            return reader;
        }






        public static string prepare_to_sql(string a)
        {
            string p;
            p = ("'" + a + "'");
            return p;
        }
    }
}
