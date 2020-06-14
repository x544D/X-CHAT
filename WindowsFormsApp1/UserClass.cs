using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IbtiChat
{


    public class Themes
    {

        private int th_id;
        public Themes(int id)
        {
            th_id = id;
        }


    }

    public enum UserState : uint
    {
        LoggedIn,
        LoggedOut,
        InitialState
    }

    public enum LogState : int
    {
        isLoggedOut,
        isLoggedIn
    }


    public class UserClass
    {

        private string strCon = Properties.Settings.Default.strCon;


        // hadi bach n3rfo wach deja loggina wlla no wlla dar logged out
        public UserState uState { set; get; }

        public int Id { set; get; }
        public string FullName { set; get; }
        public int Age { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public int TotalMsgs { set; get; }
        public bool  Banned { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public Themes Theme { set; get; }
        public bool IsAdmin { set; get; }


        public UserClass() { uState = UserState.InitialState; }


        public bool SetLogStatus(LogState LogStatus)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(strCon))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Users set isLogged = @log WHERE id = @id", cn);
                    cmd.Parameters.AddWithValue("@log", LogStatus);
                    cmd.Parameters.AddWithValue("@id", this.Id);
                    cn.Open();
                    if (cmd.ExecuteNonQuery() >= 1) return true; else return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool SetUserInfos(DataTable dt)
        {
            try
            {
                if (((int)dt.Rows[0][9]) == 1) 
                {
                    MessageBox.Show("[+] You are Perma banned !");
                    Logout();
                    return false;
                }
                Id = (int)dt.Rows[0][0];
                FullName = (string)dt.Rows[0][1];
                Age = (int)dt.Rows[0][2];
                UserName = (string)dt.Rows[0][3];
                Password = (string)dt.Rows[0][4];
                TotalMsgs = (int)dt.Rows[0][5];
                Phone = dt.Rows[0][6].ToString();
                Email = (string)dt.Rows[0][7];
                Theme = new Themes((int)dt.Rows[0][8]);
                Banned = ((int)dt.Rows[0][9]) == 0 ? false : true; // maybe add ban reasons table


                // no need for isLogged Field cuz it is implemented in client > dt[10]
                IsAdmin = (bool)dt.Rows[0][11]; // maybe add ban reasons table

                uState = UserState.LoggedIn; // logged in
                SetLogStatus(LogState.isLoggedIn);// UPDATE DB


                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message+"\n"+ex.StackTrace);
                return false;
            }
        }

        public void Logout()
        {
            uState = UserState.LoggedOut; // hadi au niv d'app
            SetLogStatus(LogState.isLoggedOut); // au niv server
        }

        public UserClass(int id, string fullName, int age, string userName, string password, int totalMsgs, string phone, string email, Themes theme, bool banned, bool isAdmin = false)
        {
            Id = id;
            FullName = fullName;
            Age = age;
            UserName = userName;
            Password = password;
            TotalMsgs = totalMsgs;
            Banned = banned;
            Phone = phone;
            Email = email;
            Theme = theme;
            IsAdmin = isAdmin;

            uState = UserState.LoggedIn; // logged in
            SetLogStatus(LogState.isLoggedIn);// UPDATE DB        
        }

        public override string ToString()
        {
            return base.ToString();
        }

        
    }
}
