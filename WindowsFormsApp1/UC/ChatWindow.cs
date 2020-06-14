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
using System.Data.SqlClient;

namespace IbtiChat.UC
{
    public partial class ChatWindow : UserControl
    {
        int txtBoxWidth = 651;
        
        List<Thread> appThreads;
        
        List<MessageClass> msgs = new List<MessageClass>();
        GLOBAL_STUFF go ;
        UserClass LoggedUser;
        ScrollBar vScrollz = new VScrollBar()
        {
            Visible = false,
            
        };

        int _odd = 1;

        public ChatWindow(ref List<Thread> appThreads, ref UserClass user, ref GLOBAL_STUFF go  )
        {

            this.appThreads = appThreads;

            if (user != null)
            {
                LoggedUser = user;
                InitializeComponent();
            }
            else this.Dispose();

            if (go != null)
            {
                this.go = go;
            }
            else
            {
                MessageBox.Show("[~] GO was derefrenced  => Err  [ChatWindow] 0x30");
                LoggedUser.Logout();
                Environment.Exit(0);
            }


                       
        }

        private void ChatWindow_Load(object sender, EventArgs e)
        {

            if (LoggedUser.IsAdmin)
            {
                txtBoxWidth = 509;
                adminPanel.Visible = true;
                adminPanel.Enabled = true;

            }

            sendMessageTxtBox.Width = txtBoxWidth;

            //dgvOnlineUsers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //dgvOnlineUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dgvOnlineUsers.RowHeadersVisible = false;
            //dgvOnlineUsers.ColumnHeadersVisible = false;
            //dgvOnlineUsers.Columns[0].Width = dgvOnlineUsers.Parent.Width / 4;
            //dgvOnlineUsers.AllowUserToAddRows = false;
            //dgvOnlineUsers.AllowUserToDeleteRows = false;
            //dgvOnlineUsers.AllowUserToOrderColumns = false;
            //dgvOnlineUsers.AllowUserToResizeColumns = false;
            //dgvOnlineUsers.Visible = false;
            


            Thread OnlineUserThread = new Thread(new ThreadStart(async () =>
            {
                //var OnlineUsers = new List<object>().Select(t => new { Id = default(int), fullName = default(string) } ).ToList();
                //dgvOnlineUsers.DataSource = OnlineUsers;

        

                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.strCon))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * from Users WHERE isLogged = 1", cn);
                    await cn.OpenAsync();

   
                    while (true)
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while(await reader.ReadAsync())
                            {
                                if (((int)reader["id"]) == LoggedUser.Id && ((int)reader["isBan"]) == 1)
                                {
                                    LoggedUser.Banned = true;

                                }
                                //OnlineUsers.Add(new { Id = (int)reader["id"], fullName = (string)reader["fullName"] });
                            }
                            //if (dgvOnlineUsers.Rows.Count < 1  && OnlineUsers.Count > 0) // first use
                            //{
                            //    foreach (var item in OnlineUsers) dgvOnlineUsers.Rows.Add(new object[] { item.Id, item.fullName });
                            //}

                            //foreach (DataGridViewRow row in dgvOnlineUsers.Rows)
                            //{
                            //    if (!OnlineUsers.Contains(new { Id=(int)row.Cells[0].Value , fullName = (string)row.Cells[1].Value }))
                            //    {
                            //        dgvOnlineUsers.Rows.Remove(row);
                            //        dgvOnlineUsers.Refresh();
                            //        dgvOnlineUsers.Invalidate();
                            //    }
                            //}


                        }
                        Thread.Sleep(1000);
                    }
                }
            }))
            { Name = "OnlineUserThread"};
            appThreads.Add(OnlineUserThread);
            OnlineUserThread.Start();


            Thread WorldChatRefresher = new Thread(new ThreadStart(() =>
            {
                bool isFirstTime = true;
                while (true)
                {

                    if (msgs.Count == 0) isFirstTime = true;
                    int newMsgsCount = go.GetWorldMessages(ref msgs);

                    if (newMsgsCount > 0)
                    {
                        // exmpl :
                        // list dyalna fiha 4 total
                        // 2 msgs jaw jdad 
                        // donc bach n3rf ghi jdad  => List.count  - 2 = 2 => index = 2 - 1
                        int _tIcounter = isFirstTime == true ? 0 : msgs.Count - newMsgsCount ;

                        isFirstTime = false;

                        worldChatWindow.BeginInvoke( new Action(async() =>
                        {


                            for (int i = _tIcounter; i < msgs.Count; i++)
                            {
                                bool isMe = msgs[i]._From == LoggedUser.Id ? true : false;
                                UserPrototype _tmpUser = await go.GetUserById(msgs[i]._From);

                                Panel p = new Panel
                                {

                                    Width = worldChatWindow.Width,
                                    Dock = DockStyle.Top,
                                    Height = 80,
                                    BackColor = _odd % 2 == 0 ? Color.LightGray : Color.LightSlateGray,
                                    Parent = worldChatWindow,
                                    Name = "hTest",
                                    Tag = msgs[i].Id
                                };

                                PictureBox pb = new PictureBox
                                {
                                    Location = new Point(10, 0),
                                    Parent = p,
                                    Width = 100,
                                    Height = p.Height,
                                    BackColor = Color.White,
                                    BorderStyle = BorderStyle.FixedSingle,
                                    Margin = new Padding(0, 0, 10, 0),
                                    BackgroundImageLayout = ImageLayout.Center,
                                    BackgroundImage = _tmpUser.IsAdmin ? Properties.Resources.uAdmin : null,
                                    Tag = new Dictionary<string, object>()
                                    {
                                        { "user" ,_tmpUser },
                                        { "message", msgs[i]}
                                    },
                                };

                                if (LoggedUser.IsAdmin)
                                {
                                    ContextMenu cm = new ContextMenu();

                                    cm.MenuItems.Add("Copy Message", new EventHandler((o, ev) =>
                                    {
                                        Dictionary<string, object> _TempDic = pb.Tag as Dictionary<string, object>;
                                        Clipboard.SetText(((MessageClass)_TempDic["message"])._Content);
                                    }));

                                    cm.MenuItems.Add("Copy Username", new EventHandler((o, ev) =>
                                    {
                                        Dictionary<string, object> _TempDic = pb.Tag as Dictionary<string, object>;
                                        Clipboard.SetText(((UserPrototype)_TempDic["user"]).UserName);
                                    }));

                                    cm.MenuItems.Add("Delete Message", new EventHandler(async (o, ev) =>
                                    {
                                        Dictionary<string, object> _TempDic = pb.Tag as Dictionary<string, object>;
                                        DialogResult res = MessageBox.Show($"+ User : {((UserPrototype)_TempDic["user"]).FullName}\n+ Message : {((MessageClass)_TempDic["message"])._Content}", "Confirmez La suppression ?", MessageBoxButtons.YesNo);

                                        if (res == DialogResult.Yes)
                                        {
                                            bool filt = await ((MessageClass)_TempDic["message"]).BlackFilterMessage();
                                            if (filt)
                                            {
                                                MessageBox.Show("Message BlackListed successfully !");
                                                worldChatWindow.Focus();
                                                worldChatWindow.Invalidate();

                                            }
                                            else MessageBox.Show("Failed To filter the Message !");
                                        }
                                    }));

                                    cm.MenuItems.Add("Ban User", new EventHandler(async (o, ev) =>
                                    {
                                        Dictionary<string, object> _TempDic = pb.Tag as Dictionary<string, object>;
                                        DialogResult res = MessageBox.Show($"+ Ban User : {((UserPrototype)_TempDic["user"]).FullName}", "Confirmez Le Ban ?", MessageBoxButtons.YesNo);

                                        if (res == DialogResult.Yes)
                                        {
                                            bool banned = await ((UserPrototype)_TempDic["user"]).BanUser();
                                            if (banned)
                                            {
                                                MessageBox.Show("User is banned successfully !");
                                                worldChatWindow.Invalidate();

                                            }
                                            else MessageBox.Show("Failed to ban user ");
                                        }
                                    }));

                                    pb.ContextMenu = cm;
                                }

                                pb.Paint += (o, v) =>
                                {
                                    v.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                    Graphics g = v.Graphics;

                                    Random rnd = new Random();

                                    Brush brush = new SolidBrush(go.GetRandomColor());
                                    Pen pen = new Pen(brush);

                                    StringFormat sf = new StringFormat();
                                    sf.LineAlignment = StringAlignment.Center;
                                    sf.Alignment = StringAlignment.Center;
                                    Rectangle  rectangle = new Rectangle(pb.Location, new Size(80, p.Height));


                                    if (!_tmpUser.IsAdmin)
                                    {
                                        g.DrawString(isMe ? "</>" : _tmpUser.FullName[0].ToString(), new Font("Tahoma", isMe ? 20f : 40f, FontStyle.Bold, GraphicsUnit.Pixel), brush, rectangle, sf);
                                    }

                                };

                                RichTextBox rt = new RichTextBox
                                {
                                    Parent = p,
                                    Location = new Point(110, 0),
                                    Width = p.Width - 120,
                                    Height = 104,
                                    MaximumSize = p.Size,
                                    BackColor = Color.WhiteSmoke,
                                    BorderStyle = BorderStyle.None,
                                    ReadOnly = true,
                                    Enabled = false,
                                    Text = msgs[i]._Content

                                };


                                Label lb = new Label
                                {
                                    Parent = pb,
                                    TextAlign = ContentAlignment.MiddleCenter,
                                    Text = isMe ? "[ M O I ]" : _tmpUser.FullName
                                };

                                pb.Controls.Add(lb);
                                p.Controls.Add(rt);
                                p.Controls.Add(pb);
                                worldChatWindow.Controls.Add(p);
                                worldChatWindow.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 10, Width = p.Width, BackColor = Color.Transparent });
                                _odd += 1;

                                worldChatWindow.Focus();
                            }

                        }));
                    }
                    Thread.Sleep(500);
                }

            }))
            { Name = "WorldChatRefresher"};
            appThreads.Add(WorldChatRefresher);
            WorldChatRefresher.Start();


            Thread RealTime_Filtrer = new Thread(new ThreadStart(async () =>
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.strCon))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * from MMessages Where isFiltred = 1", cn);
                    await cn.OpenAsync();

                    //Dictionary<int, int> FiltredMessages = new Dictionary<int, int>();
                    List<int> FiltredMessagesIds = new List<int>();

                    int OldSize = 0;


                    while (true)
                    {

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int pId = (int)reader["id"];
                                int? c = FiltredMessagesIds.Where(k => k == pId).FirstOrDefault();

                                if ( c == 0 || c == null)
                                {
                                    FiltredMessagesIds.Add(pId);
                                }
                            }

                        }


                        if (OldSize < FiltredMessagesIds.Count)
                        {
                            OldSize = FiltredMessagesIds.Count;

                            worldChatWindow.BeginInvoke(new Action(() =>
                            {
                                foreach (Control control in worldChatWindow.Controls)
                                {
                                    if (control is Panel && !control.IsDisposed && control.Name == "hTest")
                                    {
                                       
                                        try
                                        {
                                            int tagId = (int)control.Tag;
                                            if (FiltredMessagesIds.FirstOrDefault(id => id == tagId) > 0)
                                            {
                                                control.Focus();
                                                control.Dispose();
                                                control.Invalidate();
                                                worldChatWindow.Invalidate();
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            worldChatWindow.Invalidate();
                                        }
                                    }
                                }
                            }));
                        }

                        Thread.Sleep(1000);
                    }
                }
            }))
            { Name = "RealTime_Filtrer" };
            appThreads.Add(RealTime_Filtrer);
            RealTime_Filtrer.Start();



            //worldChatWindow.AutoScroll = false;
            worldChatWindow.HorizontalScroll.Enabled = false;
            worldChatWindow.HorizontalScroll.Visible = false;
            worldChatWindow.HorizontalScroll.Maximum = 0;
            worldChatWindow.AutoScroll = true;
            worldChatWindow.MouseWheel += scrollMousy;


            sendMessageTxtBox.KeyDown += (o, k) => {
                
                if (sendMessageTxtBox.Lines.Length > 7)
                {
                    k.SuppressKeyPress = true;
                }
            };

            sendMessageTxtBox.TextChanged += (o, oo) =>
            {

                if (string.IsNullOrEmpty(sendMessageTxtBox.Text))
                {
                    SendBtn.Enabled = false;
                }
                else SendBtn.Enabled = true;
            };
        }

        private void scrollMousy(object o , MouseEventArgs v)
        {
            worldChatWindow.Focus();
            if (v.Delta > 0)
            {
                if (worldChatWindow.VerticalScroll.Value - 11 >= worldChatWindow.VerticalScroll.Minimum)
                    worldChatWindow.VerticalScroll.Value -= 11;
                else
                    worldChatWindow.VerticalScroll.Value = worldChatWindow.VerticalScroll.Minimum;
            }
            else
            {
                if (worldChatWindow.VerticalScroll.Value + 11 <= worldChatWindow.VerticalScroll.Maximum)
                    worldChatWindow.VerticalScroll.Value += 11;
                else
                    worldChatWindow.VerticalScroll.Value = worldChatWindow.VerticalScroll.Maximum;
            }
        }
   
        private void VScrollz_Scroll(object sender, ScrollEventArgs e)
        {
            worldChatWindow.VerticalScroll.Value = vScrollz.Value;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(sendMessageTxtBox.Text) ) // extra check who knows
            {
                string _t = sendMessageTxtBox.Text;
                sendMessageTxtBox.Clear();
                sendMessageTxtBox.Enabled = false;

                bool res = await go.SendMessage(_t, LoggedUser.Id);
                sendMessageTxtBox.Enabled = true;

                if (res)
                {
                    worldChatWindow.Focus();
                    worldChatWindow.VerticalScroll.Value = 0;
                    worldChatWindow.Invalidate();
                    //sendMessageTxtBox.Focus();
                }
            }
        }

        private void dgvOnlineUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void worldChatWindow_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sendMessageTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private async void deleteAllMessages_Click(object sender, EventArgs e)
        {
            await go.FilterAllMessagesAndClearChat();
        }

        private async void shutDownServer_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.strCon))
            {
                
                SqlCommand cmd = new SqlCommand("UPDATE _SERVER set up = @state_", cn);
                bool _temp = go.IsServerOn;

                cmd.Parameters.AddWithValue("@state_", !go.IsServerOn);

                await cn.OpenAsync();
                if (await cmd.ExecuteNonQueryAsync() > 0)
                {
                    if (_temp)
                    {
                        MessageBox.Show("Serveur Deconnecte avec success !\nTous On etaient Deconnectes saud les admins .");
                    }
                    else
                    {
                        MessageBox.Show("Serveur Connected avec success !");
                    }
                }
               
            }
        }
    }
}
