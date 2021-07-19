using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ipcam
{
    class setCameraModel
    {
       /* private String modulator;
        private String source;*/
       public String getSource()
        {
            String connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alfalak\source\repos\AlertSystem\AlertSystem\Database1.mdf;Integrated Security=True";
            cnn = new SqlConnection(connetionString);
            try
            {
                String source = "";//textBox1.Text;
                SqlCommand command = new SqlCommand("select source from camera where Id=2", cnn);
                cnn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                   while (reader.Read())
                    {
                        source +=reader[0].ToString();
                    }
                    return source;

                }


            }
            catch
            {
                MessageBox.Show("Connection to db not success  !");
                return "";
            }
            finally
            {
                cnn.Close();
            }
        }
        public void setSource(String Source)
        {
            String connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alfalak\source\repos\AlertSystem\AlertSystem\Database1.mdf;Integrated Security=True";
            cnn = new SqlConnection(connetionString);
            try
            {
                String source = Source;//textBox1.Text;
                SqlCommand command = new SqlCommand("update camera set source='" + source + "' where Id=2", cnn);
                cnn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    MessageBox.Show("Done !");

                }


            }
            catch
            {
                MessageBox.Show("Connection to db not success  !");
            }
            finally
            {
                cnn.Close();
            }
        }
    }
}
