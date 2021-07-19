using ipcam;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlertSystem
{
    public partial class login : Form
    {
        reportModel ReportModel;
        WaitFormFunc waitForm = new WaitFormFunc();
        public login()
        {
            InitializeComponent();
            ReportModel = new reportModel();
        }

        private void button1_Click(object sender, EventArgs e) // submit button
        {
            waitForm.Show(this);
            if (textBox1.Text == null || textBox1.Text == "")
            {
                waitForm.Close();
                MessageBox.Show("Username Required!");
            }
            else if (textBox2.Text == null || textBox2.Text == "")
            {
                waitForm.Close();
                MessageBox.Show("Password Required!");
            }
            else
            {
                String connetionString;
                SqlConnection cnn;
                connetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alfalak\source\repos\AlertSystem\AlertSystem\Database1.mdf;Integrated Security=True";
                cnn = new SqlConnection(connetionString);
                try
                {
                    String userName = textBox1.Text;
                    String Password = textBox2.Text;
                    SqlCommand command = new SqlCommand("select * from admin where username ='" + userName + "' and password ='" + Password + "'", cnn);
                    cnn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        String result = null;
                        while (reader.Read())
                        {
                            result = result + (String.Format("{0},{1}", reader[0], reader[1]));
                        }
                        if (result == null)
                        {
                           // waitForm.Close();
                            MessageBox.Show("wrong Username or Password!");
                        }
                        else
                        {
                            //correct !
                          //  waitForm.Close();
                            new adminPanel().Show();
                            this.Hide();
                        }

                    }


                }
                catch
                {
                   // waitForm.Close();
                    MessageBox.Show("Connection to db not success  !");
                }
                finally
                {
                    waitForm.Close();
                    cnn.Close();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            new CreateAdmin().Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        { // exit label
            Application.Exit();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

      
   
    }
      
}
