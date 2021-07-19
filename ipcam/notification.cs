using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AlertSystem
{
    public partial class notification : Form
    {
        public notification()
        {
            InitializeComponent();
            loadEmails();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // back
            new adminPanel().Show();
            this.Hide();
        }
        private void loadEmails()
        {
            String connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alfalak\source\repos\AlertSystem\AlertSystem\Database1.mdf;Integrated Security=True";
            cnn = new SqlConnection(connetionString);
            try
            {

                SqlCommand command = new SqlCommand("select email from admin", cnn);
                cnn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // MessageBox.Show("conn estab.");

                    int count = 0;
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader[0].ToString());
                        comboBox2.Items.Add(reader[0].ToString());


                    }
                    // MessageBox.Show("record added.");
                }


            }
            catch
            {
                MessageBox.Show("Eror conn.");
            }
            finally
            {
                cnn.Close();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            String from = comboBox1.Text; // email from
            String to = comboBox1.Text; // email to
            int timer = ((int)numericUpDown1.Value);
            if (to == "" && from == "")
            {
                MessageBox.Show("you should change at least one field to submit.");
            }
           else
            {
                String connetionString;
                SqlConnection cnn;
                connetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alfalak\source\repos\AlertSystem\AlertSystem\Database1.mdf;Integrated Security=True";
                cnn = new SqlConnection(connetionString);
                try
                {
                    SqlCommand command = new SqlCommand("update notify set send_from ='"+from+"',send_to = '"+to+"',timer_sec="+timer+" where adminId =1",
                        cnn);
                    cnn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        MessageBox.Show("Emails Updated");
                    }
                }
                catch
                {
                    MessageBox.Show("Error accured");
                }
                finally
                {
                    cnn.Close();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
