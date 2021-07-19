using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AlertSystem
{
    public partial class reports : Form
    {
        // String connetionString = @"Data Source=(LocalDb)\MSSqlLocalDb;Initial Catalog=logindb;Integrated Security=True;Pooling=False";
        private static String query;
        public reports()
        {
            InitializeComponent();
            query = "select * from reports";

            add_rows();
            //clear_table();
        }
       /* private void set_query(String Query)
        {
            query = Query;
        }*/
      
        private void clear_table()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows.Clear();//.Remove(dataGridView1.Rows[0]);

            }
        }

        private void add_rows()
        {
            String connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alfalak\source\repos\AlertSystem\AlertSystem\Database1.mdf;Integrated Security=True";
            cnn = new SqlConnection(connetionString);
            try
            {
              
                SqlCommand command = new SqlCommand(query,cnn);
                cnn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                   // MessageBox.Show("conn estab.");
                    

                  while (reader.Read())
                    {
                   
                        dataGridView1.Rows.Add(reader[0].ToString(),
                            reader[1].ToString(),
                            reader[2].ToString(),
                            reader[3].ToString(),
                            reader[4].ToString()
                            );
                    }
                }
                    
               
            }
            catch
            {
                MessageBox.Show("Bad Search.");
            }
            finally
            {
                cnn.Close();
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // back
            new adminPanel().Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             // grid click
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Output.csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dataGridView1.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dataGridView1.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dataGridView1.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dataGridView1.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dataGridView1.Rows[i - 1].Cells[j].Value.ToString() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("Data Exported Successfully !!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e) // refresh btn
        {
            clear_table();
            query = "select* from reports";
            add_rows();
        }


        private void button2_Click(object sender, EventArgs e)// search btn
        {
           String selected =  comboBox1.Text.ToString();
            String value = textBox1.Text.ToString();
            query = " select * from reports where "+ selected+ " like'"+ value+"%'";
             clear_table();
             add_rows();
        }
    }
}
