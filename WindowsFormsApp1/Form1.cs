using IbtiChat.UC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IbtiChat
{
    public partial class mainWindow : Form
    {

        UserClass LoggedUser = new UserClass();
        GLOBAL_STUFF go;
        List<Thread> appThreads = new List<Thread>();
        bool IsServerOn = true;


        public mainWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MainPanel.MouseHover += (o, ev) =>
            {
                MessageBox.Show("On");
            };

            go = new GLOBAL_STUFF(ref IsServerOn, this);
            titleBar.MouseDown  += go.MouseDown;
            titleBar.MouseUp    += go.MouseUp;
            titleBar.MouseMove  += go.MouseMove;

            LoginUC  loginInterface = new LoginUC(ref appThreads ,ref LoggedUser, ref go) {
                Parent = MainPanel,
                Dock = DockStyle.Fill
            };

            MainPanel.Controls.Add(loginInterface);


            Thread auth_thread = new Thread(new ThreadStart(async() =>
            {
                
                while (true)
                {

                    await go.ServerCheckerAndKicker();
                    if (!go.IsServerOn && !LoggedUser.IsAdmin)
                    {
                        if (LoggedUser.uState == UserState.LoggedIn)
                        {
                            LoggedUser.Logout();
                        }

                            MainPanel.BeginInvoke(new Action(() =>
                            {
                                //MainPanel.Focus();
                                MainPanel.Invalidate();

                            }));
                        }
                    
                        // extra check
                        if (LoggedUser.uState == UserState.LoggedIn && LoggedUser.Banned)
                        {
                            LoggedUser.Logout();
                            MessageBox.Show("[+] You are Perma banned !");

                        }

                        if (LoggedUser.uState == UserState.LoggedOut)
                        {
                            MainPanel.BeginInvoke(new Action(() =>
                            {
                                foreach (Control control in MainPanel.Controls)
                                {
                                    control.Dispose();
                                }
                                LoggedUser = new UserClass();
                                MainPanel.Controls.Add(new LoginUC(ref appThreads ,ref LoggedUser, ref go  ) { Parent = MainPanel, Dock = DockStyle.Fill });
                            }));

                            disconnectBtn.BeginInvoke(new Action(() =>
                            {
                                disconnectBtn.Visible = false;
                            }));

                            //adminPanel.BeginInvoke(new Action(() =>
                            //{
                            //    adminPanel.Visible = false;
                            //    adminPanel.Enabled = false;
                            //}));
                        }
                        else if (LoggedUser.uState == UserState.LoggedIn)
                        {
                            disconnectBtn.BeginInvoke(new Action(() =>
                            {
                                disconnectBtn.Visible = true;
                            }));

                            //if (LoggedUser.IsAdmin)
                            //{
                            //    adminPanel.BeginInvoke(new Action(() =>
                            //    {
                            //        adminPanel.Visible = true;
                            //        adminPanel.Enabled = true;
                            //    }));
                            //}
                        }
                        else
                        {
                            disconnectBtn.BeginInvoke(new Action(() =>
                            {
                                disconnectBtn.Visible = false;
                            }));

                        }



                        Thread.Sleep(200);
                    }
                }))
                { Name = "auth_thread" };
            appThreads.Add(auth_thread);
            auth_thread.Start();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            LoggedUser.Logout();
            Environment.Exit(0);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void disconnectBtn_Click(object sender, EventArgs e)
        {
            foreach (Thread thread in appThreads)
            {
                if (thread.Name != "auth_thread")
                {
                    thread.Abort();
                }
            }
            LoggedUser.Logout();
        }



    }
}
