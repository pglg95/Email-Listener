using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D.Net.EmailClient;
using D.Net.EmailInterfaces;

namespace Email_Listener
{
   public class MClient
    {
        public IEmailClient client_obj { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string imap { get; set; }
        public MClient(IEmailClient client_obj,string login,string password,string imap)
       {
           this.client_obj = client_obj;
           this.login = login;
           this.password = password;
           this.imap = imap;
       }
        public void connect(int port,bool ssl)
        {
            client_obj.Connect(imap,login,password,port,ssl);
        }
        public void set_folder(string name)
        {
            client_obj.SetCurrentFolder(name);
        }
        public void load_emails(int nr)
        {
            client_obj.LoadRecentMessages(nr);
        }
       public void load_emails()
        {
            client_obj.LoadMessages();
        }
       public void disconnect()
       {
           client_obj.Disconnect();
       }
    }
}
