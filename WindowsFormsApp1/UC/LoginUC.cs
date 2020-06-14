using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace IbtiChat.UC
{
    public partial class LoginUC : UserControl
    {
        GLOBAL_STUFF go;
        UserClass LoggedUser;
        List<Thread> appThreads;
        //Panel WorldChatPanelRef;

        public LoginUC(ref List<Thread> appThreads, ref UserClass LoggedUser , ref GLOBAL_STUFF go  )
        {
            this.appThreads = appThreads;
            this.LoggedUser = LoggedUser ?? new UserClass();
            this.go = go;
            InitializeComponent();
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            usernameTxtBox.Clear();
            passTxtBox.Clear();
        }

        private void ConnectBtn_Click(object sender, EventArgs e)
        {
            // no time to make checks for security ... injection ..etc
            object res = go.CheckLogin(usernameTxtBox.Text, passTxtBox.Text);

            if (res == null)
            {
                MessageBox.Show("Informations Invalides !");
            }
            else
            {
                if(LoggedUser.SetUserInfos((DataTable)res))
                {
                    ChatWindow chwnd = new ChatWindow(ref appThreads, ref LoggedUser , ref go )
                    {
                        Parent = Parent,
                        Dock = DockStyle.Fill
                    };
                    Parent.Controls.Add(chwnd);
                    Dispose();
                }
            }
        }

        private void LoginUC_Load(object sender, EventArgs e)
        {
            if (!go.IsServerOn && !LoggedUser.IsAdmin)
            {
                usernameTxtBox.Text = "Server Down ... ";
                //usernameTxtBox.Enabled = false;
                passTxtBox.Text = "Server Down ... ";
                //passTxtBox.Enabled = false;
                //ConnectBtn.Enabled = false;
                //ClearBtn.Enabled = false;
            }
            else
            {
                usernameTxtBox.Clear();
                passTxtBox.Clear();
                //usernameTxtBox.Enabled = true;
                //passTxtBox.Enabled = true;
                //ConnectBtn.Enabled = true;
                //ClearBtn.Enabled = true;
            }
        }
    }
}
