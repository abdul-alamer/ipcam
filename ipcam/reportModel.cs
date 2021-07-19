using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ipcam
{
    class reportModel
    {
       private String connetionString;
     private  SqlConnection cnn;
        public reportModel() {
            connetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alfalak\source\repos\AlertSystem\AlertSystem\Database1.mdf;Integrated Security=True";
            cnn = new SqlConnection(connetionString);
        }
        public void setTitle(String title)
        {

        }
        public void setPerson(String person)
        {

        }
        public void setCameraSource(String source) 
        {

        }
        public void setAll(String title,String person , String time,String source) 
        {
            try
            { 
                SqlCommand command = new SqlCommand(
 "insert into reports (threat_title,person_name,time,camera_id)" +
 " values ('" + title + "','" + person + "','" + time + "','" + source + "')", cnn);
                cnn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    MessageBox.Show("report added to db.");
                }
            }
            
            catch
            {
                MessageBox.Show("error connection or query");
            }
            finally
            {
                cnn.Close();
            }

        }
    }
}
