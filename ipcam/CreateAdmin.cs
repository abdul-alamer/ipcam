using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
using ipcam;

namespace AlertSystem
{
    public partial class CreateAdmin : Form
    {
      static adminModel AdminModel;
    security_code sec;
        WaitFormFunc waitForm = new WaitFormFunc();
        public CreateAdmin()
        {
            InitializeComponent();
            panel1.Show();// = true;
            panel2.Hide();
            panel3.Hide();
            AdminModel = new adminModel();
          //  security_code sec;// = generateRndom();
        }
      private struct security_code // 
        {
            private static String sec_code = generateRndom();
          public void  setSecurity_code()
            {
                sec_code = generateRndom();
            }
            public String getSecurity_code()
            {
                return sec_code;
            }
            
        }

        private void label8_Click(object sender, EventArgs e) // exit
        {
            Application.Exit(); // Application.Restart()
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private static void sendEmail(String subject, String body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("abdulrahmansadaqa@gmail.com", "24534801"),
                EnableSsl = true,
            };

            smtpClient.Send("abdulsadaqa1@gmail.com",AdminModel.getEmail(), subject, body);
        }
        private static String generateRndom()
        {
            System.Random random = new System.Random();
            String code = random.Next(10000, 59999).ToString();
            return code;
        }
            private void button3_Click(object sender, EventArgs e)// panel 3 verify
        {
            //   MessageBox.Show(security_code);
            String UserValue = textBox8.Text.ToString();
            if (String.Equals(UserValue, sec.getSecurity_code()))
            {
               
                String connetionString;
                SqlConnection cnn;
                connetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alfalak\source\repos\AlertSystem\AlertSystem\Database1.mdf;Integrated Security=True";
                cnn = new SqlConnection(connetionString);
                //insert into admin (username,password,email,full_name,phone) values ('mohd99','moh123','mohd@hmail.com','mohd jamal','+99888555');
                try
                {
                    String fullname = AdminModel.getFullname();
                    String phone = AdminModel.getPhone();
                    String username = AdminModel.getUsername();
                    String passsword = AdminModel.getPassword();
                    String email = AdminModel.getEmail();
                    SqlCommand command = new SqlCommand(
 "insert into admin (username,password,email,full_name,phone) values ('" + username + "','"+ passsword+"','"+email+"','"+fullname+"','"+phone+"')", cnn);

                    cnn.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    MessageBox.Show("Admin created.");
                   
                    new login().Show();
                    this.Hide();
                }
                catch
                {
                    MessageBox.Show("Error connection.");
                }
                finally
                {
                    cnn.Close();
                }
            }
            else
                MessageBox.Show("Wrong verfication code.");
            /*  MessageBox.Show("Email:"+AdminModel.getEmail()+"\n name:"+AdminModel.getFullname()+"\n pass:"+
                AdminModel.getPassword()+"\n username:"+AdminModel.getUsername()+"\n phone:"+AdminModel.getPhone()
                  +"\n     done!.");*/



        }

        private void button2_Click(object sender, EventArgs e) //p2 u&p
        {
            String username = textBox6.Text.ToString();
            String password = textBox5.Text.ToString();
            String confirm = textBox4.Text.ToString();
            if (username == "" || password == "" || confirm == "")
                MessageBox.Show("You Should Complete the required Fields.");
            else if (!String.Equals(password,confirm))
                MessageBox.Show("Password confirmation Wrong");
            else if (!adminModel.validateUsername(username))
                MessageBox.Show("Username Already Exist.");
            else
            {
                //    MessageBox.Show("Done.");
                AdminModel.setUsername(username);
                AdminModel.setPassword(password);
                panel1.Hide();
                 panel2.Hide();
                 panel3.Show();
                sendEmail("Admin Account", "To finish setting up this account, we just need to make sure this email address is yours.\n security code :" + sec.getSecurity_code());
                MessageBox.Show("check your email to get your verification code.");
            }
        }

        private void button1_Click(object sender, EventArgs e) //p1 name ,#,email
        {
            String fullname = textBox1.Text.ToString();
            String phone = textBox2.Text.ToString();
            String email = textBox3.Text.ToString();

            if (fullname == "" || phone == "" || email == "" )
                MessageBox.Show("You Should Complete the required Fields.");
            else if (!adminModel.IsValidEmail(email))
                MessageBox.Show("Enter A Valid Email.");
            else
            {
                AdminModel.setEmail(email);
                AdminModel.setFullname(fullname);
                AdminModel.setPhone(phone);
             panel1.Hide();// = true;
             panel2.Show();
             panel3.Hide();

            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {
            // resend
            sec.setSecurity_code();
            sendEmail("Admin Account", "To finish setting up this account, we just need to make sure this email address is yours.\n security code :"
                + sec.getSecurity_code());

            MessageBox.Show("check your Email Please.");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            new login().Show();
            this.Hide();
        }
    }
}
