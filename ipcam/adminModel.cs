using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AlertSystem
{
    class adminModel
    {
        private static String connetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alfalak\source\repos\AlertSystem\AlertSystem\Database1.mdf;Integrated Security=True";
        private static SqlConnection cnn = new SqlConnection(connetionString);
        private String Fullname;
        private String Email;
        private String Phone;
        private String Password;
        private String username;

        public adminModel()
        {
            Fullname = "";
            Email = "";
            Phone = "";
            Password = "";
            username = "";
        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static bool validateUsername(String Username)
        {
            try
            {
                SqlCommand command = new SqlCommand("select * from admin where username='"+Username+"'", cnn);
                cnn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    String Temp = "";
                    while (reader.Read())
                    {
                        Temp += reader[0].ToString();
                    }
                    if (Temp == "")
                        return true;
                    else
                        return false;
                }
            }
            catch
            {
                MessageBox.Show("err conn in validate.");
                return false;
            }
            finally
            {
                cnn.Close();
            }
        }
        public void setUsername(String Username)
        {
            username = Username;
        }
        public void setPassword(String password)
        {
            Password = password;
        }
        public void setPhone(String phone)
        {
            Phone = phone;
        }
        public void setEmail(String email)
        {
            Email = email;
        }
        public void setFullname (String fullname)
        {
            Fullname = fullname;
        }
        public String getUsername()
        {
            return username;
        }
        public String getPassword()
        {
            return Password;
        }
        public String getPhone()
        {
            return Phone;
        }
        public String getEmail()
        {
            return Email;
        }
        public String getFullname()
        {
            return Fullname;
        }
    }
}
