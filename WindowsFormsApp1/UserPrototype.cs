using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace IbtiChat
{
    [Serializable]
    public class UserPrototype
    {

        public int Id { set; get; }
        public string FullName { set; get; }
        public int Age { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public int TotalMsgs { set; get; }
        public bool Banned { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public Themes Theme { set; get; }
        public int IsLogged { set; get; }
        public bool IsAdmin { set; get; }

        public UserPrototype(int id, string fullName, int age, string userName, string password, int totalMsgs, bool banned, string phone, string email, Themes theme, int isLogged, bool isAdmin)
        {
            Id = id;
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
            Age = age;
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            TotalMsgs = totalMsgs;
            Banned = banned;
            Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Theme = theme ?? throw new ArgumentNullException(nameof(theme));
            IsLogged = isLogged;
            IsAdmin = isAdmin;
        }


        public UserPrototype(SqlDataReader dt)
        {
            try
            {
                Id = (int)dt["id"];
                FullName = (string)dt["fullName"];
                Age = (int)dt["age"];
                UserName = (string)dt["username"];
                Password = (string)dt["password"];
                TotalMsgs = (int)dt["totalmsgs"];
                Phone = (string)dt["phone"];
                Email = (string)dt["email"];
                Theme = new Themes((int)dt["theme"]);
                Banned = ((int)dt["isBan"]) == 0 ? false : true;
                IsLogged = (int)dt["isLogged"];
                IsAdmin = (bool)dt["isAdmin"];
            }
            catch (Exception) { }
        }

        public async Task<bool> BanUser()
        {
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.strCon))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("UPDATE users SET isBan=@isBan WHERE id=@id", cn);
                    SqlCommand cmd2 = new SqlCommand("UPDATE MMessages SET isFiltred = 1 WHERE _from=@id", cn);
                    cmd.Parameters.AddWithValue("@isBan", 1);
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd2.Parameters.AddWithValue("@id", Id);

                    await cn.OpenAsync();
                    int rowAffected = await cmd.ExecuteNonQueryAsync();
                    int rowAffected2 = await cmd2.ExecuteNonQueryAsync();


                    if (rowAffected == 1)
                    {
                        return true;
                    }
                    else return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            
        }
    }
}
