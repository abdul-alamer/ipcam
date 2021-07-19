using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace AlertSystem
{

    class notifyModel
    {
        private static String connetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alfalak\source\repos\AlertSystem\AlertSystem\Database1.mdf;Integrated Security=True";
         private static SqlConnection cnn = new SqlConnection(connetionString);
        private int Time;
        private String send_to;
        private String send_from;
       public notifyModel()
        {
            Time = 0;
            send_from = "";
            send_to = "";
        }
        public int getTime()
        {
            try
            {
                SqlCommand command = new SqlCommand("select timer_sec from notify", cnn);
                cnn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Time = reader.GetInt32(0);
                    }

                }
                return Time;

            }
            catch
            {
                MessageBox.Show("err conn in getTime().");
                return 0;
            }
            finally
            {
                cnn.Close();
            }
        }
        public String getSend_to()
        {
            try
            {
                SqlCommand command = new SqlCommand("select send_to from notify", cnn);
                cnn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        send_to += reader[0].ToString();
                    }

                }
                return send_to;

            }
            catch
            {
                MessageBox.Show("err conn in getSent_to().");
                return "";
            }


        }
        public String getSend_from()
        {
            try
            {
                SqlCommand command = new SqlCommand("select send_from from notify", cnn);
                cnn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        send_from += reader[0].ToString();
                    }

                }
                return send_from;

            }
            catch
            {
                MessageBox.Show("err conn in get SendFrom().");
                return "";
            }
        }

    }


}
