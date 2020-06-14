using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbtiChat
{
    public class MessageClass
    {
        public int Id { set; get; }
        public string _Content { set; get; }
        public int _From { set; get; }
        public int _To { set; get; }
        public DateTime _datetime { set; get; }
        public int _isFiltred { set; get; }

        public MessageClass(int id, string Content, int From, object To, DateTime datetime , int isFiltred)
        {
            Id = id;
            _Content = Content;
            _From = From;
            if (To is DBNull)
            {
                _To = -1;
            }
            else _To = Convert.ToInt32(To.ToString());

            _datetime = datetime;
            _isFiltred = isFiltred;
        }


        public async Task<bool> BlackFilterMessage()
        {
            using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.strCon))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("UPDATE MMessages SET isFiltred = 1  Where id = @id", cn);
                    cmd.Parameters.AddWithValue("@id", Id);

                    await cn.OpenAsync();
                    int rowAffected = await cmd.ExecuteNonQueryAsync();

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
