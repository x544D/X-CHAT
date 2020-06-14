using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IbtiChat
{

    public enum ChatType : int
    {
        WorldChat,
        PrivateChat
    }

    public class GLOBAL_STUFF
    {

        private string strCon = Properties.Settings.Default.strCon;
        public bool IsServerOn = true;

        private bool isMoving = false;
        private Point clickPoz;
        public Form refForm;
        private static Random random = new Random();

        public GLOBAL_STUFF(ref bool isServerOn, Form f)
        {
            IsServerOn = isServerOn;

            if (!f.IsDisposed)
            {
                refForm = f;
            }
        }

        // Get Color
        public Func<Color> GetRandomColor = () => Color.FromArgb(255, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

        // General title bar moving
        public void MouseDown(object o  , MouseEventArgs m)
        {
            clickPoz = m.Location;
            isMoving = true;
        }
        public void MouseUp(object o, MouseEventArgs m)
        {
            clickPoz = m.Location;
            isMoving = false;

        }
        public void MouseMove(object o, MouseEventArgs m)
        {
            if (isMoving)
            {
                int dx = clickPoz.X - m.Location.X;
                int dy = clickPoz.Y - m.Location.Y;

                refForm.Location = new Point(refForm.Location.X - dx, refForm.Location.Y-dy);
            }
        }



        public async Task ServerCheckerAndKicker()
        {
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.strCon))
            {
                try
                {

                    SqlCommand cmd = new SqlCommand("SELECT top(1) up FROM _SERVER ", cn);
                    SqlCommand cmd2 = new SqlCommand(" UPDATE Users SET isLogged = 0 WHERE isAdmin = 0 ", cn);

                    await cn.OpenAsync();
                    IsServerOn = (bool) await cmd.ExecuteScalarAsync();
                    //MessageBox.Show(IsServerOff.ToString());
                    if (!IsServerOn)
                    {
                        await cmd2.ExecuteNonQueryAsync();
                    }
                    
                }
                catch (Exception)
                {}

            }
        }

        // filter all messages
        public async Task FilterAllMessagesAndClearChat()
        {
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.strCon))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("UPDATE MMessages SET isFiltred = 1", cn);
                    await cn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    MessageBox.Show("[o] Filtred n cleared all messages , Fresh chat !");
                    
                }
                catch (Exception)
                {
                    MessageBox.Show("[x] Failed to Filter all Chat Messages !");
                }
            }
        }


        // with async programming
        public async Task<UserPrototype> /*object*/ GetUserById(int id)
        {
            DataTable tb = new DataTable();

            using (SqlConnection cn = new SqlConnection(strCon))
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT * From Users WHERE id=@id" , cn);
                    cmd.Parameters.AddWithValue("@id", id);
                    await cn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new UserPrototype(reader);
                        }
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }


        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">Message String content</param>
        /// <param name="from">Id of sendr in UserClass</param>
        /// <param name="to">Id of reciever , leave null yla knti ghatsiifti f worldChat</param>
        /// <param name="chType">ChType</param>
        /// <returns></returns>
        public async Task<bool> SendMessage(string content, int from , int? to = null)
        {
            using (SqlConnection cn = new SqlConnection(strCon))
            {
                try
                {

                    SqlCommand cmd;
                    if (to == (int)ChatType.WorldChat || to == null) // world
                    {
                        cmd = new SqlCommand("INSERT into MMessages VALUES(@cnt , @fk_sender, NULL , @time , 0)", cn);
                        cmd.Parameters.AddWithValue("@cnt", content);
                        cmd.Parameters.AddWithValue("@fk_sender", from);
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                    }
                    else // private chat 
                    {
                        cmd = new SqlCommand("INSERT into MMessages VALUES(@cnt , @fk_sender, @fk_reciever , @time, 0)", cn);
                        cmd.Parameters.AddWithValue("@cnt", content);
                        cmd.Parameters.AddWithValue("@fk_sender", from);
                        cmd.Parameters.AddWithValue("@fk_reciever", to);
                        cmd.Parameters.AddWithValue("@time", DateTime.Now);
                    }

                    await cn.OpenAsync();

                    
                    if (await cmd.ExecuteNonQueryAsync() >= 1) return true;
                    else return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        // GetMsgs

        public int GetWorldMessages(ref List<MessageClass> MsgsArrayRef) // return int = how much new row added
        {
            int id_ = 0;
            int ret_ = 0;

            if (MsgsArrayRef.Count >= 1)
            {
                id_ = MsgsArrayRef.Last().Id;
            }

            using (SqlConnection cn = new SqlConnection(strCon))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT * From MMessages WHERE isFiltred = 0 AND id > @id_ ORDER BY mTime ASC", cn);
                    cmd.Parameters.AddWithValue("@id_", id_);

                    cn.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    ret_ = dt.Rows.Count;
                   
;                   foreach (DataRow item in dt.Rows)
                    {
                        MessageClass _tmp = new MessageClass((int)item[0], item[1].ToString(), (int)item[2], item[3], (DateTime)item[4], (int)item[5]);
                        MsgsArrayRef.Add(_tmp);
                    }
                    return ret_;
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.StackTrace);
                    return ret_;
                }
            }
        }

        // LOGIN - register


        public object CheckLogin(string username, string password)
        {
            object ret = null;
            using (SqlConnection cn = new SqlConnection(strCon))
            {
                SqlCommand cmd = new SqlCommand("SELECT * from Users where username=@uname and password=@pass", cn);
                cmd.Parameters.AddWithValue("@uname", username);
                cmd.Parameters.AddWithValue("@pass", password);
                cn.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if(rd.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(rd);
                    ret = dt;
                }

            }

            return ret;
        }

        public object CheckRegister(string username, string password)
        {


            return null;
        }


    }
}
